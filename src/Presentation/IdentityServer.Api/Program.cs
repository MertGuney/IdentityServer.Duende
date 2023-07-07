var corsPolicyName = "IdentityServerCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.AddApplicationLayer();

builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddInfrastructureLayer(builder.Configuration, builder.Environment);

var authenticationBuilder = builder.Services.AddAuthentication();

var authOptions = builder.Configuration.GetSection("Authentication").Get<AuthOptions>();

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

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
});

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(opts =>
    {
        opts.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDevelopmentDatabaseAsync();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
