
using Serilog;
using ItemService.Api;

/// <summary>
/// Entry point for ItemService API. Configures and starts the web application.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Register application services, infrastructure, and configuration
builder.AddApiServices(); // Mongo, repositories, application services
builder.AddSerilog(); // Serilog logging configuration
builder.AddControllersAndSwagger(); // Controllers and Swagger/OpenAPI

var app = builder.Build();

// Configure middleware pipeline
app.UseApiMiddlewares(); // Exception handling, request logging
app.UseApiSwagger(); // Swagger UI in development
app.UseApiDefaults(); // HTTPS, authorization, controller mapping

app.Run(); // Start the web application
