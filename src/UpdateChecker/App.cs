using System.Text;
using Application.Services;
using Domain.Constants;
using Domain.Models;
using Microsoft.Extensions.Hosting;

namespace UpdateChecker;

public class App(
    IFeedService feedService,
    INotificationService notificationService,
    IFileService fileService,
    IHttpClientService httpClient,
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
                while (!cancellationToken.IsCancellationRequested)
                    await MainLoop();

            }, cancellationToken);
        });
        return Init();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task Init()
    {
        var credentials = fileService.GetFileDataAsync<ModelCredentials>(
            Directories.CredentialsFile);
        _oldFeed = File.Exists(Directories.FeedFile)
            ? await fileService.GetFileDataAsync<ModelFeed>(Directories.FeedFile)
            : await httpClient.GetAsync();
        await credentials;
    }

    private async Task MainLoop()
    {
        var newFeed = await httpClient.GetAsync();

        if (feedService.CheckUpdate(_oldFeed, newFeed, out var newChapters))
        {
            Console.WriteLine("Found new chapters.");
            var message = new StringBuilder();
            await fileService.SaveCredentialsAsync(Directories.FeedFile, newChapters);

            foreach (string chapter in newChapters)
            {
                message.AppendLine($"{ChapterUrl}/{chapter}");
            }
            var notification = notificationService.Notify(message.ToString());
            Console.WriteLine("Sending mail.");
            await notification;
        }

        _oldFeed = newFeed;
        await Task.Delay(new TimeSpan(1, 0, 0));
    }
}