namespace IdentityServer.Infrastructure.Services;
public class MailService : IMailService
{
    private readonly ILogger<MailService> _logger;

    public MailService(ILogger<MailService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendAsync(string to, string subject, string body)
    {
        try
        {
            var smtpClient = GetSmtpClient();
            var message = GetMailMessage(to, subject, body);
            await smtpClient.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while mail sending.");
            return false;
        }
    }

    public async Task<bool> SendAsync(List<string> addresses, string subject, string body)
    {
        try
        {
            var smtpClient = GetSmtpClient();
            var message = GetMailMessage(addresses, subject, body);
            await smtpClient.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while mail sending.");
            return false;
        }
    }

    public async Task<bool> SendEmailConfirmationMailAsync(string to, string userId, string token)
    {
        return await SendAsync(to, "Email Confirmation", $"{userId}/{token}");
    }

    public async Task<bool> SendResetPasswordMailAsync(string to, string userId, string token)
    {
        return await SendAsync(to, "Reset Password", $"{userId}/{token}");
    }

    public async Task<bool> SendForgotPasswordMailAsync(string to, string userId, string token)
    {
        return await SendAsync(to, "Forgot Password", $"{userId}/{token}");
    }

    private static SmtpClient GetSmtpClient()
    {
        return new SmtpClient("host", 25)
        {
            EnableSsl = false,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("username", "password")
        };
    }

    private static MailMessage GetMailMessage(string to, string subject, string body)
    {
        MailMessage message = new()
        {
            Body = body,
            Subject = subject,
            IsBodyHtml = true,
            From = new MailAddress("xxx@mail.com")
        };
        message.To.Add(new MailAddress(to));

        return message;
    }

    private static MailMessage GetMailMessage(List<string> addresses, string subject, string body)
    {
        MailMessage message = new()
        {
            Body = body,
            Subject = subject,
            IsBodyHtml = true,
            From = new MailAddress("xxx@mail.com")
        };

        foreach (var address in addresses)
        {
            message.To.Add(new MailAddress(address));
        }

        return message;
    }
}
