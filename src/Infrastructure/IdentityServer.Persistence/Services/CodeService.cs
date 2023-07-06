namespace IdentityServer.Persistence.Services;

public class CodeService : ICodeService
{
    private readonly IUserCodesRepository _userCodesRepository;

    public CodeService(IUserCodesRepository userCodesRepository)
    {
        _userCodesRepository = userCodesRepository;
    }

    public async Task<bool> VerifyAsync(Guid userId, string code, CodeTypeEnum type, CancellationToken cancellationToken)
    {
        return await _userCodesRepository.VerifyAsync(userId, code, type, cancellationToken);
    }

    public async Task<string> GenerateAsync(Guid userId, CodeTypeEnum type, CancellationToken cancellationToken)
    {
        Random random = new();
        var code = random.Next(100000, 999999);

        AspNetUserCode aspNetUserCode = new(code.ToString(), userId, type);
        aspNetUserCode.AddCreator(userId.ToString());

        return await _userCodesRepository.CreateAsync(aspNetUserCode, cancellationToken)
            ? code.ToString()
            : null;
    }
}
