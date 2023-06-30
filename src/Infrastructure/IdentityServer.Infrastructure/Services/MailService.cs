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

    public async Task<bool> SendAsync(string to, Guid userId, string code, CodeTypeEnum codeType)
    {
        bool result = false;
        switch (codeType)
        {
            case CodeTypeEnum.Register:
                result = await SendEmailConfirmationMailAsync(to, userId, code);
                break;
            case CodeTypeEnum.ChangeEmail:
                result = await SendChangeEmailConfirmationMailAsync(to, userId, code);
                break;
            case CodeTypeEnum.ChangePassword:
                result = await SendChangePasswordMailAsync(to, userId, code);
                break;
            case CodeTypeEnum.ForgotPassword:
                result = await SendForgotPasswordMailAsync(to, userId, code);
                break;
        }
        return result;
    }

    public async Task<bool> SendEmailConfirmationMailAsync(string to, Guid userId, string code)
    {
        return await SendAsync(to, "Email Confirmation", $"{userId}/{code}");
    }

    public async Task<bool> SendChangeEmailConfirmationMailAsync(string to, Guid userId, string code)
    {
        return await SendAsync(to, "Email Confirmation", $"{userId}/{code}");
    }

    public async Task<bool> SendChangePasswordMailAsync(string to, Guid userId, string code)
    {
        return await SendAsync(to, "Reset Password", $"{userId}/{code}");
    }

    public async Task<bool> SendForgotPasswordMailAsync(string to, Guid userId, string code)
    {
        return await SendAsync(to, "Forgot Password", $"{userId}/{code}");
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
