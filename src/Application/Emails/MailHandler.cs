using Domain.Interfaces;
using Domain.Models;
using Domain.Models.JSON;
using MailKit.Net.Smtp;
using MimeKit;

namespace Application.Emails;

public sealed class MailHandler(IJsonFetcher fetcher) : INotifier
{
    public async Task Notify(string message)
    {
        var model = await fetcher.GetJsonDataAsync<ModelMailInfo>(Constants.MailFile);
        var mailMessage = CreateMessage(message, model);
        using var client = new SmtpClient();
        await client.ConnectAsync(model.Host, model.Port);
        await client.AuthenticateAsync(model.From, model.Password);
        await client.SendAsync(mailMessage);
        Console.WriteLine("Sent email");
    }

    private static MimeMessage CreateMessage(string message, ModelMailInfo model)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(model.FromName, model.From));
        mailMessage.To.Add(new MailboxAddress(model.ToName, model.To));
        mailMessage.Subject = Constants.Subject;
        mailMessage.Body = new TextPart("plain") { Text = message };
        return mailMessage;
    }
}