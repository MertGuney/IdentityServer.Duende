namespace IdentityServer.Application.Features.Commands.Auth.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly ICodeService _codeService;

    public VerifyEmailCommandHandler(IUserService userService, ICodeService codeService)
    {
        _userService = userService;
        _codeService = codeService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(VerifyEmailCommandRequest request, CancellationToken cancellationToken)
    {
        User user = await _userService.GetByEmailAsync(request.Email);
        if (user is null) return ResponseModel<NoContentModel>.UserNotFound();

        bool isVerifiedCode = await _codeService.VerifyAsync(user.Id, request.Code, CodeTypeEnum.Register, cancellationToken);
        if (!isVerifiedCode)
            return await ResponseModel<NoContentModel>
                .FailureAsync(
                FailureTypes.UNVERIFIED_CODE,
                "Code unverified.",
                Guid.NewGuid().ToString(),
                StatusCodes.Status400BadRequest);

        return await _userService.VerifyEmailAsync(user);
    }
}
