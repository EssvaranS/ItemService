using ItemService.Api.Middlewares;
using ItemService.Application.IServices;
using ItemService.Infrastructure;
using ItemService.Infrastructure.IRepositories;
using ItemService.Infrastructure.Repositories;
using ItemService.Infrastructure.Settings;
using ItemService.Infrastructure.UnitOfWork;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configuration: support appsettings and environment variables
builder.Configuration.AddEnvironmentVariables();


// Bind MongoSettings (from appsettings.json or env vars)
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection(MongoSettings.SectionName));

// Register MongoDbContext
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return new MongoDbContext(settings);
});

// Repositories and UnitOfWork
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application services
builder.Services.AddScoped<IItemService, ItemService.Application.Services.ItemService>();

// Add controllers
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//// Read port from env variable PORT or default 3000
//var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
//app.Urls.Clear();
//app.Urls.Add($"http://*:{port}");

app.Run();
