using Application.Services;
using Application.Services.Implementations;
using Infrastructure.Files;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UpdateChecker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddScoped<IFileService, FileService>()
    .AddScoped<IHttpClientService, MangadexService>()
    .AddScoped<IFeedService, FeedService>()
    .AddScoped<INotificationService, EmailService>()
    .AddHostedService<App>();

var app = builder.Build();
await app.RunAsync();

