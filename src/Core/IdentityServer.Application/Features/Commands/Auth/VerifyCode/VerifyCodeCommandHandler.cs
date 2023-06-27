namespace IdentityServer.Application.Features.Commands.Auth.VerifyCode;

public class VerifyCodeCommandHandler : IRequestHandler<VerifyCodeCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly ICodeService _codeService;

    public VerifyCodeCommandHandler(IUserService userService, ICodeService codeService)
    {
        _userService = userService;
        _codeService = codeService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(VerifyCodeCommandRequest request, CancellationToken cancellationToken)
    {
        User user = await _userService.GetByEmailAsync(request.Email);

        return await _codeService.VerifyAsync(user.Id, request.Code, request.CodeType, cancellationToken)
            ? await ResponseModel<NoContentModel>.SuccessAsync()
            : await ResponseModel<NoContentModel>.FailureAsync(1, "CodeNotVerified", "An error occurred while validating the code.", StatusCodes.Status500InternalServerError);
    }
}
