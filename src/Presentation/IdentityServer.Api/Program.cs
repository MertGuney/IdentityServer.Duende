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

authenticationBuilder.AddGoogle("Google", opts =>
{
    opts.ClientId = "790647006486-n4666sbm1i13n56smle2b4niassfp7gm.apps.googleusercontent.com";
    opts.ClientSecret = "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";
    opts.SignInScheme = IdentityConstants.ExternalScheme;
    opts.Scope.Add(JwtClaimTypes.Profile);
});
//authenticationBuilder.AddTwitter(opts =>
//{
//    opts.ConsumerKey = "";
//    opts.ConsumerSecret = "";
//});
//authenticationBuilder.AddFacebook(opts =>
//{
//    opts.ClientId = "";
//    opts.ClientSecret = "";
//    opts.Fields.Add(JwtClaimTypes.Picture);
//});

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
