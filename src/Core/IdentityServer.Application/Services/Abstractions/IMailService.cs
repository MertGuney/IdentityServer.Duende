namespace IdentityServer.Application.Services.Abstractions;

public interface IMailService
{
    Task<bool> SendAsync(string to, string subject, string body);
    Task<bool> SendAsync(List<string> addresses, string subject, string body);
    Task<bool> SendEmailConfirmationMailAsync(string to, string userId, string token);
}

