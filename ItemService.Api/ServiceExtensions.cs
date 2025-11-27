using ItemService.Api.Middlewares;
using ItemService.Application.IServices;
using ItemService.Infrastructure;
using ItemService.Infrastructure.IRepositories;
using ItemService.Infrastructure.Options;
using ItemService.Infrastructure.Repositories;
using ItemService.Infrastructure.UnitOfWork;
using Microsoft.Extensions.Options;
using Serilog;

namespace ItemService.Api;

/// <summary>
/// Provides extension methods for configuring services and middleware in the ItemService API.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Registers application, infrastructure, and MongoDB services.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    public static void AddApiServices(this WebApplicationBuilder builder)
    {
        // Add environment variables to configuration
        builder.Configuration.AddEnvironmentVariables();

        // Bind MongoDB settings from configuration
        builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection(MongoOptions.SectionName));

        // Register MongoDbContext as singleton
        builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
            return new MongoDbContext(settings);
        });

        // Register repositories and unit of work
        builder.Services.AddScoped<IItemRepository, ItemRepository>();
        builder.Services.AddScoped<IUnitOfWork>(sp =>
        {
            var itemRepo = sp.GetRequiredService<IItemRepository>();
            var dbContext = sp.GetRequiredService<IMongoDbContext>();
            return new UnitOfWork(itemRepo, dbContext);
        });

        // Register application services
        builder.Services.AddScoped<IItemService, ItemService.Application.Services.ItemServices>();
    }

    /// <summary>
    /// Configures Serilog logging for the application.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    public static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
    }

    /// <summary>
    /// Registers controllers and Swagger/OpenAPI services.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    public static void AddControllersAndSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(); // Add MVC controllers
        builder.Services.AddEndpointsApiExplorer(); // Add API explorer for Swagger
        builder.Services.AddSwaggerGen(); // Add Swagger generator
    }

    /// <summary>
    /// Configures middleware for request logging and exception handling.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    public static void UseApiMiddlewares(this WebApplication app)
    {
        app.UseSerilogRequestLogging(); // Log HTTP requests
        app.UseMiddleware<ExceptionHandlingMiddleware>(); // Global exception handler
    }

    /// <summary>
    /// Enables Swagger UI in development environment.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    public static void UseApiSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // Enable Swagger middleware
            app.UseSwaggerUI(); // Enable Swagger UI
        }
    }

    /// <summary>
    /// Configures default middleware for HTTPS, authorization, and controller mapping.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    public static void UseApiDefaults(this WebApplication app)
    {
        app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
        app.UseAuthorization(); // Enable authorization
        app.MapControllers(); // Map controller endpoints
    }
}
