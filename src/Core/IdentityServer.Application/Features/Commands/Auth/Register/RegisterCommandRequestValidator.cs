namespace IdentityServer.Application.Features.Commands.Auth.Register;
public class RegisterCommandRequestValidator : AbstractValidator<RegisterCommandRequest>
{
    public RegisterCommandRequestValidator()
    {
        RuleFor(x => x.UserName).MinimumLength(8);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Email).NotEmpty().Must(MailRegex.IsValidEmail).WithMessage("Invalid email addess");
    }
}
