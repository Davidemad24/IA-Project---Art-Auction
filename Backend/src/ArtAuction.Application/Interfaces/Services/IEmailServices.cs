namespace ArtAuction.Application.Interfaces.Services;

public interface IEmailServices
{
    Task SendEmail(string email, string subject, string message);
}