namespace Domain.Services.Abstracts;

public interface IEmailSenderService
{
    public Task SendEmailAsync(string email, string subject, string message);
}