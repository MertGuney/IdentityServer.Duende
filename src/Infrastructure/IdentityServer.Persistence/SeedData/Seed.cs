namespace IdentityServer.Persistence.SeedData;

public static class Seed
{
    private static async Task CreateRolesAsync(RoleManager<Role> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new Role()
            {
                Name = "Administrator"
            });
            await roleManager.CreateAsync(new Role()
            {
                Name = "Customer"
            });
        }
    }

    private static async Task CreateUserAsync(UserManager<User> userManager)
    {
        if (!userManager.Users.Any())
        {
            User user = new()
            {
                UserName = "SuperAdmin",
                Email = "super@admin.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = "905555555555",
            };
            await userManager.CreateAsync(user, "Password*-1");
            await userManager.AddToRoleAsync(user, "Customer");
            await userManager.AddToRoleAsync(user, "Administrator");
        }
    }

    public static async Task InitializeDevelopmentDatabaseAsync(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

        var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        await appDbContext.Database.MigrateAsync();

        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        await Seed.CreateRolesAsync(roleManager);

        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
        await Seed.CreateUserAsync(userManager);
    }
}
