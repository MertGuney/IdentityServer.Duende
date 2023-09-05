namespace IdentityServer.Application.Features.Commands.Auth.TFAs.Activate;

public class ActivateTFACommandHandler : IRequestHandler<ActivateTFACommandRequest, ResponseModel<NoContentModel>>
{
    private readonly ITFAService _tfaService;
    private readonly IUserService _userService;

    public ActivateTFACommandHandler(ITFAService tfaService, IUserService userService)
    {
        _tfaService = tfaService;
        _userService = userService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(ActivateTFACommandRequest request, CancellationToken cancellationToken)
    {
        User user = await _userService.GetAsync();

        var isValidCode = await _tfaService.VerifyTwoFactorAuthCodeAsync(user, request.Code);
        if (!isValidCode)
            return await ResponseModel<NoContentModel>
                .FailureAsync(
                FailureTypes.INVALID_CODE,
                "Invalid Authentication Code",
                Guid.NewGuid().ToString(),
                StatusCodes.Status400BadRequest);

        return await _tfaService.SetTwoFactorEnabledAsync(user, true);
    }
}
