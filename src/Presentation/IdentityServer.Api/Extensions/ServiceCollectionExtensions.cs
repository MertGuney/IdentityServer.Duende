namespace IdentityServer.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureCors(this IServiceCollection services, string corsPolicyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: corsPolicyName,
                policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:3000",
                        "http://localhost:3001"
                        )
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
        });
    }

    public static void ConfigureExternalAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationBuilder = services.AddAuthentication();

        var authOptions = configuration.GetSection("Authentication").Get<AuthOptions>();

        authenticationBuilder.AddGoogle("Google", opts =>
        {
            opts.ClientId = authOptions.Google.ClientId;
            opts.ClientSecret = authOptions.Google.ClientSecret;
            opts.SignInScheme = IdentityConstants.ExternalScheme;
            opts.Scope.Add(JwtClaimTypes.Profile);
        });
        authenticationBuilder.AddTwitter(opts =>
        {
            opts.ConsumerKey = authOptions.Twitter.ClientId;
            opts.ConsumerSecret = authOptions.Twitter.ClientSecret;
        });
        authenticationBuilder.AddFacebook(opts =>
        {
            opts.ClientId = authOptions.Facebook.ClientId;
            opts.ClientSecret = authOptions.Facebook.ClientSecret;
            opts.Fields.Add(JwtClaimTypes.Picture);
        });
    }

    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityServer.Api", Version = "v1" });
            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
    }
}
