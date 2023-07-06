namespace IdentityServer.Application.Features.Commands.Auth.VerifyEmail;

public class VerifyEmailCommandRequestValidator : AbstractValidator<VerifyEmailCommandRequest>
{
    public VerifyEmailCommandRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().Must(MailRegex.IsValidEmail);
    }
}
