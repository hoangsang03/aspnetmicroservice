using BuildingBlocks.Behaviors;
using BuildingBlocks.Mapping;
using FluentValidation;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catalog.API;

public static partial class ServiceInitializer
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        RegisterMediatR(services);
        RegisterMarten(services, configuration);
        RegisterMapster(services);
        RegisterCarter(services);
        RegisterSwagger(services);
        RegisterController(services);

        return services;
    }

    private static void RegisterController(IServiceCollection services)
    {
        services.AddControllers();
    }

    private static void RegisterMediatR(IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }

    private static void RegisterMapster(IServiceCollection services)
    {
        services.AddMappings();
    }

    private static void RegisterCarter(IServiceCollection services)
    {
        services.AddCarter();
    }

    private static void RegisterSwagger(IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();

            var uri = "https://example.com/terms";
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Catalog API",
                Description = "An ASP.NET Core Web API for managing products",
                TermsOfService = new Uri(uri),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri(uri)
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri(uri)
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    private static void RegisterMarten(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMarten(opts =>
        {
            opts.Connection(configuration.GetConnectionString("PostgresqlDB")!);
        }).UseLightweightSessions();
    }
}
