namespace IdentityServer.Persistence.Repositories;

public class UserCodesRepository : IUserCodesRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<AspNetUserCode> _dbSet;
    public UserCodesRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<AspNetUserCode>();
    }

    public async Task<bool> VerifyAsync(Guid userId, string code, CodeTypeEnum type, CancellationToken cancellationToken)
    {
        return await _dbSet.AnyAsync(x =>
        x.Type == type &&
        x.Value == code &&
        x.UserId == userId &&
        x.ExpireTime >= DateTime.UtcNow,
        cancellationToken);
    }

    public async Task<bool> CreateAsync(AspNetUserCode aspNetUserCode, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(aspNetUserCode, cancellationToken);
        return await SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(AspNetUserCode aspNetUserCode, CancellationToken cancellationToken)
    {
        _dbSet.Update(aspNetUserCode);
        return await SaveChangesAsync(cancellationToken);
    }

    private async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) switch
        {
            > 0 => true,
            _ => false,
        };
    }
}
