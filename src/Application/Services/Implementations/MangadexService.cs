using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Domain.Constants;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations;

public sealed class MangadexService(
    IFileService fileService,
    ILogger<MangadexService> logger,
    IHttpClientFactory httpClientFactory) : IMangadexService
{
    private ModelCredentials _modelCredentials = fileService.GetFileData<ModelCredentials>(Directories.CredentialsFile);
    private const string MangadexApi = "https://api.mangadex.org";
    private const string MangadexAuth = "https://auth.mangadex.org/realms/mangadex/protocol/openid-connect/token";
    private const string MangadexFeed = "/user/follows/manga/feed";

    public async ValueTask<ModelFeed?> GetAsync()
    {
        var client = httpClientFactory.CreateClient("Mangadex");
        
        if (!await UpdateTokenAsync(client))
            return null;
        
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _modelCredentials.AccessToken);
        
        var uriBuilder = new UriBuilder(MangadexApi + MangadexFeed);
        uriBuilder.Query += "limit=5";
        uriBuilder.Query += "&order[publishAt]=desc";
        uriBuilder.Query += "&translatedLanguage[]=en";
        var res = await client.GetAsync(uriBuilder.ToString());
        try
        {
            res.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            logger.LogError(
                "Failed request to get a new feed at {now}.\nStatus Code: {statusCode}", DateTime.Now, e.HttpRequestError);
            return null;
        }
        var data = await res.Content.ReadAsStringAsync();
        var feed = JsonSerializer.Deserialize<ModelFeed>(data, JsonSerializerOptions.Web);

        if (feed is not null) 
            return feed;
        
        logger.LogError("Serialization for {data} failed.", data);
        return null;

    }

    private async ValueTask<bool> UpdateTokenAsync(HttpClient client)
    {
        var builder = new StringBuilder();
        builder.Append("grant_type=refresh_token");
        builder.Append($"&refresh_token={_modelCredentials.RefreshToken}");
        builder.Append($"&client_id={_modelCredentials.ClientId}");
        builder.Append($"&client_secret={_modelCredentials.ClientSecret}");
        var res = await client.PostAsync(MangadexAuth,
            new StringContent(builder.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded"));

        try
        {
            res.EnsureSuccessStatusCode();
        }

        catch (HttpRequestException e)
        {
            logger.LogError(
                "Failed request to update auth token {now}.\nStatus Code: {statusCode}", DateTime.Now, e.HttpRequestError);
            return false;
        }

        var data = await res.Content.ReadAsStringAsync();
        var newToken = JsonSerializer.Deserialize<ModelToken>(data, JsonOptions.SnakeCase);

        if (newToken == null)
        {
            logger.LogError("Failed serialization for {data} at {now}", data, DateTime.Now);
            return false;
        } 
        
        var newCredentials = _modelCredentials with { AccessToken = newToken.AccessToken };
        _modelCredentials = newCredentials;
        await fileService.SaveCredentialsAsync(Directories.CredentialsFile, _modelCredentials);
        
        return true;
    }
    
    public bool CheckUpdate(ModelFeed oldFeed, ModelFeed newFeed, out List<string> newChapters)
    {
        newChapters = [];
        
        newChapters.AddRange(
            from chapterId in newFeed.Data
            where oldFeed.Data.All(x => x!.Id != chapterId!.Id) select chapterId!.Id!);

        return newChapters.Count > 0;
    }
}