namespace IdentityServer.Application.Repositories;

public interface IUserCodesRepository
{
    Task<bool> UpdateAsync(AspNetUserCode aspNetUserCode, CancellationToken cancellationToken);

    Task<bool> CreateAsync(AspNetUserCode aspNetUserCode, CancellationToken cancellationToken);

    Task<bool> VerifyAsync(Guid userId, string code, CodeTypeEnum type, CancellationToken cancellationToken);
}
