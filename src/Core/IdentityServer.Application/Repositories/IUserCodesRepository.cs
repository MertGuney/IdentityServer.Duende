namespace IdentityServer.Application.Repositories;

public interface IUserCodesRepository
{
    Task<bool> UpdateAsync(AspNetUserCode aspNetUserCode, CancellationToken cancellationToken);

    Task<bool> CreateAsync(AspNetUserCode aspNetUserCode, CancellationToken cancellationToken);

    Task<bool> IsVerifiedCodeAsync(Guid userId, string code, CodeTypeEnum type, CancellationToken cancellationToken);

    Task<AspNetUserCode> UnverifiedCodeAsync(Guid userId, string code, CodeTypeEnum type, CancellationToken cancellationToken);
}
