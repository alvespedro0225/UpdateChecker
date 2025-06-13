using System.Net.Http.Headers;
using Application.Services;
using Application.Services.Implementations;
using Infrastructure.Files;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UpdateChecker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
        .AddScoped<IFileService, FileService>()
        .AddScoped<IMangadexService, MangadexService>()
        .AddScoped<INotificationService, EmailService>()
        .AddHostedService<App>();

builder.Services
        .AddHttpClient("Mangadex", client =>
        {
                client.DefaultRequestHeaders.UserAgent.Add(
                        new ProductInfoHeaderValue("UpdateChecker", "1.0"));
        });
// appsettings.json not working fix later
builder.Logging.SetMinimumLevel(LogLevel.Error);
// TODO add testing, review models
var app = builder.Build();
await app.RunAsync();

