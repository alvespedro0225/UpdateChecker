using Application.Services.Implementations;
using Domain.Models;
using Infrastructure.Files;
using Constants = Domain.Models.Constants;
using JsonSerializer = System.Text.Json.JsonSerializer;

var fileManager = new FileService();
var credentials = await fileManager.GetCredentialsAsync<ModelCredentials>(Constants.CredentialsFile);
var client = new MangadexService(credentials, fileManager);
ModelFeed? oldFeed;

if (File.Exists(Path.Combine(Constants.Path, Constants.FeedFile)))
    oldFeed = await fileManager.GetCredentialsAsync<ModelFeed>(Constants.FeedFile);
else
{
    var res = await client.GetAsync();
    oldFeed = JsonSerializer.Deserialize<ModelFeed>(res, Constants.JsonOptions);
    await fileManager.SaveCredentialsAsync(Constants.FeedFile, oldFeed);
}

var mailHandler = new MailService(fileManager);
var feedService = new FeedService();
while (true)
{
    var response = await client.GetAsync();
    var newFeed = JsonSerializer.Deserialize<ModelFeed>(response, Constants.JsonOptions);
    if (!feedService.CheckUpdate(oldFeed!, newFeed!, out var newChapters))
    {
        Console.WriteLine("Found new chapters.");
        var message = string.Empty;
        await fileManager.SaveCredentialsAsync(Constants.FeedFile, newFeed);

        foreach (string chapter in newChapters)
        {
            message += $"{Constants.ChapterUrl}/{chapter}\n";
        }

        Console.WriteLine("Sending mail.");
        await mailHandler.Notify(message);
    }

    oldFeed = newFeed;
    await Task.Delay(new TimeSpan(1, 0, 0));
}