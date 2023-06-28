namespace IdentityServer.Application.Features.Commands.Auth.TFAs.Deactivate;

public class DeactivateTFACommandHandler : IRequestHandler<DeactivateTFACommandRequest, ResponseModel<NoContentModel>>
{
    private readonly ITFAService _tfaService;
    private readonly IUserService _userService;

    public DeactivateTFACommandHandler(ITFAService tfaService, IUserService userService)
    {
        _tfaService = tfaService;
        _userService = userService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(DeactivateTFACommandRequest request, CancellationToken cancellationToken)
    {
        User user = await _userService.GetAsync();

        var isValidCode = await _tfaService.VerifyTwoFactorAuthCodeAsync(user, request.Code);
        if (!isValidCode)
            return await ResponseModel<NoContentModel>.FailureAsync(1,
                "InvalidCode",
                "Invalid Authentication Code",
                StatusCodes.Status400BadRequest);

        return await _tfaService.SetTwoFactorEnabledAsync(user, false);
    }
}
