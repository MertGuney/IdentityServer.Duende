namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendChangePasswordCode;

public class SendChangePasswordCodeCommandRequestValidator : AbstractValidator<SendChangePasswordCodeCommandRequest>
{
    public SendChangePasswordCodeCommandRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().Must(MailRegex.IsValidEmail);
    }
}
