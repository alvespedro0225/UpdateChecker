using System.Net.Http.Headers;
using Application.Services;
using Application.Services.Implementations;
using Infrastructure.Files;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UpdateChecker;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services => services
        .AddScoped<IFileService, FileService>()
        .AddScoped<IHttpClientService, MangadexService>()
        .AddScoped<IFeedService, FeedService>()
        .AddScoped<INotificationService, EmailService>()
        .AddHostedService<App>()
        .AddHttpClient("Mangadex", client =>
        {
                client.DefaultRequestHeaders.UserAgent.Add(
                        new ProductInfoHeaderValue("UpdateChecker", "1.0"));
        }));

await builder.RunConsoleAsync();

