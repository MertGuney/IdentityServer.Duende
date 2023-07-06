namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendChangeEmailCode;

public class SendChangeEmailCodeCommandRequestValidator : AbstractValidator<SendChangeEmailCodeCommandRequest>
{
    public SendChangeEmailCodeCommandRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().Must(MailRegex.IsValidEmail);
    }
}
