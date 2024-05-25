namespace LittleLemon_API.Services.EmailServices;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlMessage);
}