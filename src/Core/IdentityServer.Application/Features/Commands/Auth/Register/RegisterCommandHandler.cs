namespace IdentityServer.Application.Features.Commands.Auth.Register;
public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    //TODO: Wrong error code
    public async Task<ResponseModel<NoContentModel>> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
    {
        User user = new(request.Email, request.UserName);

        var registerResponse = await _authService.RegisterAsync(user, request.Password);
        if (!registerResponse.IsSuccessful) return registerResponse;

        var addUserToRoleResponse = await _authService.AddToRoleAsync(user, "Customer");
        if (!addUserToRoleResponse.IsSuccessful) return addUserToRoleResponse;

        return await _authService.SendEmailConfirmationTokenAsync(user)
            ? await ResponseModel<NoContentModel>.SuccessAsync(StatusCodes.Status201Created)
            : await ResponseModel<NoContentModel>.FailureAsync(1, "FailedToSendEmailConfirmationMail",
            "An error occurred while sending the email confirmation mail", StatusCodes.Status500InternalServerError);
    }
}
