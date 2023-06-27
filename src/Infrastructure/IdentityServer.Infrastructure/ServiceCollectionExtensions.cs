namespace IdentityServer.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        services.AddServices();

        services.AddLocalApiAuthentication();

        if (webHostEnvironment.IsDevelopment())
        {
            services.AddDevelopmentIdentity();
        }
        else
        {
            services.AddProductionIdentity(configuration, webHostEnvironment);
        }
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMailService, MailService>();
    }

    private static void AddDevelopmentIdentity(this IServiceCollection services)
    {
        services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(2));

        var identityBuilder = services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            options.Events.RaiseInformationEvents = true;

            options.EmitStaticAudienceClaim = true;
        })
            .AddInMemoryIdentityResources(Config.IdentityResources())
            .AddInMemoryApiResources(Config.ApiResources())
            .AddInMemoryApiScopes(Config.ApiScopes())
            .AddInMemoryClients(Config.Clients())
            .AddAspNetIdentity<User>();

        identityBuilder.AddProfileService<IdentityProfileService>();
        identityBuilder.AddExtensionGrantValidator<TokenExchangeExtensionGrantValidator>();
        identityBuilder.AddResourceOwnerValidator<IdentityResourceOwnerPasswordValidator>();
        identityBuilder.AddDeveloperSigningCredential();
    }

    private static void AddProductionIdentity(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        // services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.FromHours(12));

        var migrationsAssembly = "IdentityServer.Persistence";

        // Password Token Lifetime
        services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(2));

        var identityBuilder = services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            options.Events.RaiseInformationEvents = true;

            options.EmitStaticAudienceClaim = true;
        })
            .AddConfigurationStore(opts =>
            {
                opts.ConfigureDbContext = b => b.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(opts =>
            {
                opts.ConfigureDbContext = b => b.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddProfileService<IdentityProfileService>()
            .AddAspNetIdentity<User>();

        identityBuilder.AddResourceOwnerValidator<IdentityResourceOwnerPasswordValidator>();
        identityBuilder.AddExtensionGrantValidator<TokenExchangeExtensionGrantValidator>();

        var rsa = new RSAKeyService(webHostEnvironment, TimeSpan.FromDays(30));

        services.AddSingleton(provider => rsa);

        identityBuilder.AddSigningCredential(rsa.GetKey(), RsaSigningAlgorithm.RS512);
    }
}
