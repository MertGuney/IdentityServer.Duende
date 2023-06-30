namespace IdentityServer.Application.Features.Commands.Auth.SendCode;

public class SendCodeCommandRequestValidator : AbstractValidator<SendCodeCommandRequest>
{
    public SendCodeCommandRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}