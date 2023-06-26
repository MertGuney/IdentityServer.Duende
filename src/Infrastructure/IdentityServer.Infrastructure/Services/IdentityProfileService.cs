namespace IdentityServer.Infrastructure.Services;

public class IdentityProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;

    public IdentityProfileService(UserManager<User> userManager, RoleManager<Role> roleManager, IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        User user = await _userManager.FindByIdAsync(sub);
        ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

        List<Claim> claims = userClaims.Claims.ToList();
        claims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();

        if (_userManager.SupportsUserRole)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
                if (_roleManager.SupportsRoleClaims)
                {
                    Role identityRole = await _roleManager.FindByNameAsync(role);
                    if (identityRole is not null)
                    {
                        claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                    }
                }
            }
        }
        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        string sub = context.Subject.GetSubjectId();
        User user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user is not null;
    }
}
