using System.Text;
using Application.Services;
using Domain.Constants;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Hosting;

namespace UpdateChecker;

public class App(
    INotificationService notificationService,
    IFileService fileService,
    IMangadexService mangadexService,
    IHostApplicationLifetime lifetime) : IHostedService
{
    private const string ChapterUrl = "https://mangadex.org/chapter";
    private ModelFeed _oldFeed = null!;


    public Task StartAsync(CancellationToken cancellationToken)
    {
        lifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                await MainLoop(cancellationToken);

            }, cancellationToken);
        });
        return Init(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task Init(CancellationToken token)
    {
        var credentials = fileService.GetFileDataAsync<ModelCredentials>(
            Directories.CredentialsFile);
        var error = 0;
        
        while (true)
        {
            var feed = File.Exists(Directories.FeedFile)
                ? await fileService.GetFileDataAsync<ModelFeed>(Directories.FeedFile)
                : await mangadexService.GetAsync();

            if (feed is not null)
            {
                _oldFeed = feed;
                break;
            }


            if (error >= 3)
            {
                await notificationService.NotifyError(Error.FeedError);
                Environment.Exit(1);
            }
            
            error++;
            
            await Task.Delay(new TimeSpan(1, 0, 0), token);
        }
        await credentials;
    }

    private async Task MainLoop(CancellationToken token)
    {
        var errors = 0;
        while (!token.IsCancellationRequested)
        {
            var newFeed = await mangadexService.GetAsync();

            if (newFeed is null)
            {
                errors++;

                switch (errors)
                {
                    case 5:
                        await notificationService.NotifyError(Error.ConnectionError);
                        break;
                    case > 5:
                        Environment.Exit(1);
                        break;
                }
                
                await Task.Delay(new TimeSpan(1, 0, 0), token);
                continue;
            }

            errors = 0;
            
            if (mangadexService.CheckUpdate(_oldFeed, newFeed, out var newChapters))
            {
                var message = new StringBuilder();
                await fileService.SaveCredentialsAsync(Directories.FeedFile, newChapters);

                foreach (string chapter in newChapters)
                    message.AppendLine($"{ChapterUrl}/{chapter}");
                
                await notificationService.Notify(message.ToString());
            }

            _oldFeed = newFeed;
            await Task.Delay(new TimeSpan(1, 0, 0), token);
        }
    }
}
