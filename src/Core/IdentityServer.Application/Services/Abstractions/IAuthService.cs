namespace IdentityServer.Application.Services.Abstractions
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string email, string userName, string password);
        //Task<bool> ResetPasswordAsync(string email, string code, string newPassword, CancellationToken cancellationToken);
    }
}
