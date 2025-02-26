using System.Text.Json;
using updateBot.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

const string path = "/home/pedro/code/csharp/API/API";
var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
    IndentSize = 4
};

var credentialHandler = new FileCredentialsHandler(path + "/credentials.json");
var credentials = credentialHandler.GetCredentialsAsync();
var mangaLoader = new FileMangaLoader(path + "/mangas.json", jsonOptions);
var client = new MangadexClient(await credentials, credentialHandler, jsonOptions);
var oldFeed = await mangaLoader.GetDataAsync();
try
{
    var newFeed = await client.CheckFeed();
    if (DataManager.AreEqual(oldFeed, newFeed)) Console.WriteLine("Equal");
    else Console.WriteLine("Not equal");
}
catch (HttpRequestException e)
{
    if (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
    {
        await client.UpdateTokenAsync();
        var newFeed = await client.CheckFeed();
        if (DataManager.AreEqual(oldFeed, newFeed)) Console.WriteLine("Equal");
        else Console.WriteLine("Not equal");
        return;
    }
    Console.WriteLine(e.StatusCode);
    Console.WriteLine(e.Message);
}
