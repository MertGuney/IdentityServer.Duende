namespace IdentityServer.Application.Features.Commands.Users.ChangePassword;

public class ChangePasswordCommandRequestValidator : AbstractValidator<ChangePasswordCommandRequest>
{
    public ChangePasswordCommandRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty();
        RuleFor(x => x.CurrentPassword).NotEmpty();
    }
}