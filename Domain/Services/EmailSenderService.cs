using Domain.Services.Abstracts;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Domain.Services;

public class EmailSenderService : IEmailSenderService
{
    public async Task SendEmailAsync(string email, string subject, string message)

    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Администрация сайта", "viexsad@mail.ru"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(TextFormat.Html)
        {
            Text = message
        };
        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.mail.ru", 465, true);
        await client.AuthenticateAsync("viexsad@mail.ru", "hcUt5k37hR2zffvcNsWj");
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}