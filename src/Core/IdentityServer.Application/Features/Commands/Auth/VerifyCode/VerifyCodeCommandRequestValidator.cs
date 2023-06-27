namespace IdentityServer.Application.Features.Commands.Auth.VerifyCode;

public class VerifyCodeCommandRequestValidator : AbstractValidator<VerifyCodeCommandRequest>
{
    public VerifyCodeCommandRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty().Length(6);
        RuleFor(x => x.CodeType).NotEmpty().IsInEnum();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
