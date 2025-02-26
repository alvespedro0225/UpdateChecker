using Data.Models;
using updateBot.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;


var credentials = await FileManager.GetJsonFileAsync<ModelCredentials>(Constants.CredentialsFile);
var oldFeed = await FileManager.GetJsonFileAsync<ModelFeed>(Constants.MangaFile);
var client = new MangadexClient(credentials);
await client.UpdateTokenAsync();
var response = await client.CheckFeed();
var newFeed = JsonSerializer.Deserialize<ModelFeed>(response, Constants.JsonOptions);
if (newFeed == null) throw new NullReferenceException();
if (FeedHandler.AreEqual(oldFeed, newFeed)) Console.WriteLine("Equal");
else Console.WriteLine("Not equal");