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
        .AddScoped<IMangadexService, MangadexService>()
        .AddScoped<INotificationService, EmailService>()
        .AddHostedService<App>()
        );

builder.ConfigureServices(services => services
        .AddHttpClient("Mangadex", client =>
        {
                client.DefaultRequestHeaders.UserAgent.Add(
                        new ProductInfoHeaderValue("UpdateChecker", "1.0"));
        }));

// TODO add logging and testing, review models
await builder.RunConsoleAsync();

