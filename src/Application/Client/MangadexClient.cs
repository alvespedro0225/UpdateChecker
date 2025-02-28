using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.JSON;

namespace Application.Client;

public sealed class MangadexClient(ModelCredentials modelCredentials, IJsonFetcher jsonFetcher) : IClient
{
    private ModelCredentials _modelCredentials = modelCredentials;

    private readonly HttpClient _httpClient = new()
    {
        DefaultRequestHeaders =
        {
            Authorization = new AuthenticationHeaderValue("Bearer", modelCredentials.AccessToken),
            UserAgent = { new ProductInfoHeaderValue("UpdateChecker", "1.0") }
        }
    };

    public async Task<string> CheckFeedAsync()
    {
        try
        {
            await UpdateTokenAsync();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.StatusCode == null
                ? "There was an issue connecting to the server. \nPlease check your internet connection and try again."
                : e.Message);
            Environment.Exit(1);
        }

        var uriBuilder = new UriBuilder(Constants.MangadexApi + Constants.MangadexFeed);
        uriBuilder.Query += "limit=5";
        uriBuilder.Query += "&order[publishAt]=desc";
        uriBuilder.Query += "&translatedLanguage[]=en";
        var res = await _httpClient.GetAsync(uriBuilder.ToString());
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadAsStringAsync();
    }

    private async Task UpdateTokenAsync()
    {
        // would be better to use char array instead of string, but it's small enough that it doesn't matter 
        var content = "grant_type=refresh_token";
        content += $"&refresh_token={_modelCredentials.RefreshToken}";
        content += $"&client_id={_modelCredentials.ClientId}";
        content += $"&client_secret={_modelCredentials.ClientSecret}";
        var res = await _httpClient.PostAsync(Constants.MangadexAuth,
            new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));

        try
        {
            res.EnsureSuccessStatusCode();
        }

        catch (HttpRequestException e)
        {
            Console.WriteLine(e.StatusCode);
            Console.WriteLine(await res.Content.ReadAsStringAsync());
            Environment.Exit(1);
            throw;
        }

        var data = await res.Content.ReadAsStringAsync();
        var newToken = JsonSerializer.Deserialize<ModelToken>(data);
        if (newToken == null) throw new NullReferenceException();
        var newCredentials = new ModelCredentials
        {
            AccessToken = newToken.access_token,
            ClientId = _modelCredentials.ClientId,
            ClientSecret = _modelCredentials.ClientSecret,
            RefreshToken = _modelCredentials.RefreshToken
        };
        _modelCredentials = newCredentials;
        await jsonFetcher.SaveJsonDataAsync(Constants.CredentialsFile, _modelCredentials);
        UpdateHeaders();
    }

    private void UpdateHeaders()
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _modelCredentials.AccessToken);
    }
}