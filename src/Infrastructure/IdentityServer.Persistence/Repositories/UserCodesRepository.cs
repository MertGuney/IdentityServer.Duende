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

    public async Task<AspNetUserCode> UnverifiedCodeAsync(Guid userId, string code, CodeTypeEnum type, CancellationToken cancellationToken)
    {
        return await _dbSet.Where(x => x.UserId == userId && x.Value == code && x.ExpireTime >= DateTime.Now && x.Type == type && !x.IsVerified)
            .OrderBy(x => x.CreatedDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsVerifiedCodeAsync(Guid userId, string code, CodeTypeEnum type, CancellationToken cancellationToken)
    {
        return await _dbSet.AnyAsync(x => x.UserId == userId && x.Value == code && x.Type == type && x.IsVerified, cancellationToken);
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
