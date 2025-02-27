using Data.Models.JSON;
using Infrastructure.Files;
using updateBot.Client;
using Constants = Data.Models.Constants;
using JsonSerializer = System.Text.Json.JsonSerializer;
using updateBot.Emails;

var fileManager = new FileFetcher();
var credentials = await fileManager.GetJsonDataAsync<ModelCredentials>(Constants.CredentialsFile);
var oldFeed = await fileManager.GetJsonDataAsync<ModelFeed>(Constants.MangaFile);
var client = new MangadexClient(credentials, fileManager);
var response = await client.CheckFeedAsync();
var newFeed = JsonSerializer.Deserialize<ModelFeed>(response, Constants.JsonOptions);
var mailHandler = new MailHandler(fileManager);

if (newFeed == null) throw new NullReferenceException();

if (!FeedHandler.AreEqual(oldFeed, newFeed, out var newChapters))
{
    var message = string.Empty;
    await fileManager.SaveJsonDataAsync(Constants.MangaFile, newFeed);
    foreach (string chapter in newChapters)
    {
        message += $"{Constants.ChapterUrl}/{chapter}\n";
    }

    await mailHandler.Notify(message);
}