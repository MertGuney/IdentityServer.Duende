var corsPolicyName = "IdentityServerCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors(corsPolicyName);

builder.Services.AddApplicationLayer();

builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddInfrastructureLayer(builder.Configuration, builder.Environment);

builder.Services.ConfigureVersioning();

builder.Services.ConfigureExternalAuth(builder.Configuration);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(opts =>
    {
        opts.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

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
