using System.Net;
using System.Net.Mail;
using ArtAuction.Application.Interfaces.Services;

namespace ArtAuction.Infrastructure.Services;

public class EmailServices : IEmailServices
{
    public async Task SendEmail(string email, string subject, string body)
    {
        // Create smtp protocol
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("divademadpro160@gmail.com", "wovn cqyx uhff orwr"),
            EnableSsl = true
        };
        
        // Create email
        var message = new MailMessage
        {
            From = new MailAddress("divademadpro160@gmail.com"),
            To = {email},
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        // Send email
        await smtpClient.SendMailAsync(message);
    }
}