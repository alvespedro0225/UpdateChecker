using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using updateBot.Interfaces;

namespace updateBot.Models;

public sealed class MangadexClient(ModelCredentials modelCredentials,
    ICredentialsHandler credentialsHandler,
    JsonSerializerOptions jsonOptions)
{
    private ModelCredentials _modelCredentials = modelCredentials;
    private const string BaseUrl = "https://api.mangadex.org"; 
    
    private readonly HttpClient _httpClient = new HttpClient
    {
        DefaultRequestHeaders =
        {
            Authorization = new AuthenticationHeaderValue("Bearer", modelCredentials.AccessToken),
            UserAgent = { new ProductInfoHeaderValue("UpdateBot", "1.0") },
        }
    };

    public async Task<ModelFeed> CheckFeed()
    {
        var res = await _httpClient.GetAsync($"{BaseUrl}/user/follows/manga/feed?limit=5&order%5BpublishAt%5D=desc&translatedLanguage%5B%5D=en");
        res.EnsureSuccessStatusCode();
        var data = await res.Content.ReadAsStringAsync();
        var newFeed = JsonSerializer.Deserialize<ModelFeed>(data, jsonOptions);
        if (newFeed == null) throw new NullReferenceException();
        return newFeed;
    }

    public async Task UpdateTokenAsync()
    {

        var content = $"grant_type=refresh_token&refresh_token={_modelCredentials.RefreshToken}&client_id={_modelCredentials.ClientId}&client_secret={_modelCredentials.ClientSecret}";
        var res = await _httpClient.PostAsync(
            "https://auth.mangadex.org/realms/mangadex/protocol/openid-connect/token",
            new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")
            );
        try
        {
            res.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.StatusCode);
            Console.WriteLine(await res.Content.ReadAsStringAsync());
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
        await credentialsHandler.SaveCredentialsAsync(_modelCredentials);
        UpdateHeaders();
    }

    private void UpdateHeaders()
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _modelCredentials.AccessToken);
    }

}