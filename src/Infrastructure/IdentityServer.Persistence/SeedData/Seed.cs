using IdentityServer.Application.Common.Constants;

namespace IdentityServer.Persistence.SeedData;

public static class Seed
{
    private static async Task CreateRolesAsync(RoleManager<Role> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new Role()
            {
                Name = RoleConstants.Customer
            });
            await roleManager.CreateAsync(new Role()
            {
                Name = RoleConstants.Manager
            });
            await roleManager.CreateAsync(new Role()
            {
                Name = RoleConstants.Administrator
            });
        }
    }

    private static async Task CreateUserAsync(UserManager<User> userManager)
    {
        if (!userManager.Users.Any())
        {
            User user = new("super@admin.com", "SuperAdmin")
            {
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = "905555555555",
            };
            await userManager.CreateAsync(user, "Password*-1");
            await userManager.AddToRoleAsync(user, RoleConstants.Customer);
            await userManager.AddToRoleAsync(user, RoleConstants.Administrator);
        }
    }

    public static async Task InitializeDevelopmentDatabaseAsync(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

        var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        var migrations = appDbContext.Database.GetMigrations();
        var appliedMigrations = await appDbContext.Database.GetAppliedMigrationsAsync();

        if (migrations.Count() > appliedMigrations.Count()) await appDbContext.Database.MigrateAsync();


        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        await CreateRolesAsync(roleManager);

        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
        await CreateUserAsync(userManager);
    }
}
