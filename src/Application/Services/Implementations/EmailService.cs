using Domain.Constants;
using Domain.Enums;
using Domain.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Application.Services.Implementations;

public sealed class EmailService(IFileService fileService) : INotificationService
{
    public async Task Notify(string message)
    {
        const string subject = "New chapters";
        var model = await fileService.GetFileDataAsync<ModelMailInfo>(Directories.MailFile);
        var mailMessage = CreateMessage(model, subject, message);
        using var client = new SmtpClient();
        await client.ConnectAsync(model.Host, model.Port);
        await client.AuthenticateAsync(model.From, model.Password);
        await client.SendAsync(await mailMessage);
    }

    public async Task NotifyError(Error error)
    {
        var subject = Enum.GetName(error);
        subject ??= $"Unknown error: {error.ToString()}";
        var model = await fileService.GetFileDataAsync<ModelMailInfo>(Directories.MailFile);
        var mailMessage = CreateMessage(model, subject, string.Empty);
        using var client = new SmtpClient();
        await client.ConnectAsync(model.Host, model.Port);
        await client.AuthenticateAsync(model.From, model.Password);
        await client.SendAsync(await mailMessage);
    }

    private static Task<MimeMessage> CreateMessage(ModelMailInfo model, string subject, string message)
    {
        var mailMessage = new MimeMessage();
        mailMessage.Subject = Directories.Subject;
        mailMessage.Body = new TextPart("plain") { Text = message };
        mailMessage.From.Add(new MailboxAddress(model.FromName, model.From));
        mailMessage.To.Add(new MailboxAddress(model.ToName, model.To));
        return Task.FromResult(mailMessage);
    }
}