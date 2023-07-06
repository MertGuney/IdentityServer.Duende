namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendForgotPasswordCode;

public class SendForgotPasswordCodeCommandRequestValidator : AbstractValidator<SendForgotPasswordCodeCommandRequest>
{
    public SendForgotPasswordCodeCommandRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().Must(MailRegex.IsValidEmail);
    }
}
