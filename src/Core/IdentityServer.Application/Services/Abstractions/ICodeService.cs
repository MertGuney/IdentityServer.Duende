namespace IdentityServer.Application.Services.Abstractions
{
    public interface ICodeService
    {
        Task<string> GenerateAsync(Guid userId, CodeTypeEnum type, CancellationToken cancellationToken);

        Task<bool> VerifyAsync(Guid userId, string code, CodeTypeEnum type, CancellationToken cancellationToken);

        Task<bool> IsVerifiedAsync(Guid userId, string code, CodeTypeEnum type, CancellationToken cancellationToken);
    }
}
