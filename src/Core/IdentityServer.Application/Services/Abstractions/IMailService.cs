namespace IdentityServer.Application.Services.Abstractions;

public interface IMailService
{
    Task<bool> SendAsync(string to, string subject, string body);

    Task<bool> SendAsync(List<string> addresses, string subject, string body);

    Task<bool> SendChangePasswordMailAsync(string to, Guid userId, string code);

    Task<bool> SendForgotPasswordMailAsync(string to, Guid userId, string code);

    Task<bool> SendEmailConfirmationMailAsync(string to, Guid userId, string code);

    Task<bool> SendChangeEmailConfirmationMailAsync(string to, Guid userId, string code);
}

