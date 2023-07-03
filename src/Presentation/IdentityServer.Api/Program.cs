var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.InitializeDevelopmentDatabaseAsync();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
