namespace IdentityServer.Application.Services.Abstractions
{
    public interface IAuthService
    {
        Task<bool> SendEmailConfirmationTokenAsync(User user);

        Task<ResponseModel<NoContentModel>> ForgotPasswordAsync(string email);

        Task<ResponseModel<NoContentModel>> AddToRoleAsync(User user, string role);

        Task<ResponseModel<NoContentModel>> RegisterAsync(User user, string password);

        Task<ResponseModel<NoContentModel>> ResetPasswordAsync(string userId, string encodedToken, string newPassword);
    }
}
