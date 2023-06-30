namespace IdentityServer.Application.Features.Commands.Users.ChangeEmail;

public class ChangeEmailCommandRequestValidator : AbstractValidator<ChangeEmailCommandRequest>
{
    public ChangeEmailCommandRequestValidator()
    {
        RuleFor(x => x.NewEmail).NotEmpty().WithMessage("Mail address could not null.").EmailAddress().WithMessage("Mail address format is not valid.");
    }
}