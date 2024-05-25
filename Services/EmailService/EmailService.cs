using System.Net;
using System.Net.Mail;

namespace LittleLemon_API.Services.EmailServices;

// TODO: To implement with smtp
public class EmailService : IEmailService
{
    private readonly string _gmailEmail = "twojEmail@gmail.com";
    private readonly string _password = "twojeHaslo";

    public EmailService()
    {

    }

    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(_gmailEmail, _password),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_gmailEmail),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(to);

        await smtpClient.SendMailAsync(mailMessage);
    }
}