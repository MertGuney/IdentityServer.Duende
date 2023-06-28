namespace IdentityServer.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediator();
        services.AddServices();
        services.AddAutoMapper();
        services.AddValidators();
        services.AddFilterAttributes();
        services.AddExceptionHandling();
        services.AddHttpContextAccessor();
        services.AddLoggingPipelineBehaviours();
        services.AddValidationPipelineBehaviours();
        services.AddPerformancePipelineBehaviours();
        services.AddExceptionHandlerPipelineBehaviours();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }

    private static void AddExceptionHandling(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
    }

    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    private static void AddFilterAttributes(this IServiceCollection services)
    {
        services.AddScoped<UserNotFoundFilterAttribute>();
    }

    private static void AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
    }

    private static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }

    private static void AddValidationPipelineBehaviours(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static void AddLoggingPipelineBehaviours(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }

    private static void AddPerformancePipelineBehaviours(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
    }

    private static void AddExceptionHandlerPipelineBehaviours(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(ExceptionHandlingBehavior<,,>));
    }
}
