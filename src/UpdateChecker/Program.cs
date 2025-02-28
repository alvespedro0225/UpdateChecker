using Application.Client;
using Application.Emails;
using Domain.Models.JSON;
using Infrastructure.Files;
using Constants = Domain.Models.Constants;
using JsonSerializer = System.Text.Json.JsonSerializer;

var fileManager = new FileFetcher();
var credentials = await fileManager.GetJsonDataAsync<ModelCredentials>(Constants.CredentialsFile);
var client = new MangadexClient(credentials, fileManager);
ModelFeed? oldFeed;

if (File.Exists(Constants.FeedFile))
    oldFeed = await fileManager.GetJsonDataAsync<ModelFeed>(Constants.FeedFile);
else
{
    var res = await client.CheckFeedAsync();
    oldFeed = JsonSerializer.Deserialize<ModelFeed>(res, Constants.JsonOptions);
    await fileManager.SaveJsonDataAsync(Constants.FeedFile, oldFeed);
}

var mailHandler = new MailHandler(fileManager);

while (true)
{
    var response = await client.CheckFeedAsync();
    var newFeed = JsonSerializer.Deserialize<ModelFeed>(response, Constants.JsonOptions);
    if (!FeedHandler.AreEqual(oldFeed!, newFeed!, out var newChapters))
    {
        Console.WriteLine("Found new chapters.");
        var message = string.Empty;
        await fileManager.SaveJsonDataAsync(Constants.FeedFile, newFeed);

        foreach (string chapter in newChapters)
        {
            message += $"{Constants.ChapterUrl}/{chapter}\n";
        }

        Console.WriteLine("Sending mail.");
        await mailHandler.Notify(message);
    }
    else
        Console.Write("Nothing new.");

    oldFeed = newFeed;
    await Task.Delay(new TimeSpan(1, 0, 0));
}