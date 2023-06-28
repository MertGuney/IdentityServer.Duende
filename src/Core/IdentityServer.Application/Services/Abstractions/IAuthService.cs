namespace IdentityServer.Application.Services.Abstractions
{
    public interface IAuthService
    {
        Task<ResponseModel<NoContentModel>> UpdateSecurityStampAsync(User user);

        Task<ResponseModel<NoContentModel>> ResetPasswordAsync(User user, string newPassword);

        Task<ResponseModel<NoContentModel>> ForgotPasswordAsync(string email, CancellationToken cancellationToken);

        Task<ResponseModel<NoContentModel>> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}
