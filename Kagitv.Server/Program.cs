using AiclaRM.Server.Extensions;
using AIRMDataManager.Library.Common.DataAccess;
using AIRMDataManager.Library.DataAccess.MsSql;
using AIRMDataManager.Library.Modules.Tourney.Wisata.DataAccess;
using AIRMDataManager.Library.Modules.Traveloka.Wisata.DataAccess;
using AIRMDataManager.Library.SystemCoreDataAccess;
using Kagitv.Server.Models;
using Kagitv.Server.Services.Traveloka.Wisata;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure connection string in appsettings.json and get it here.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Register the database connection factory.
builder.Services.AddScoped<IDatabaseConnectionFactory>(sp =>
    new MsSqlDatabaseConnectionFactory(connectionString ?? throw new ArgumentNullException(nameof(connectionString))));

//builder.Services.AddScoped<IDatabaseConnectionFactory, OLDDatabaseConnectionFactory>();
builder.Services.AddScoped<IWisataService, WisataService>();

builder.Services.AddScoped<IWisataRepository, MsSqlWisataRepository>();

builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Configure the context to use sqlite.
    //options.UseSqlite($"Filename={Path.Combine(Path.GetTempPath(), "openiddict-dantooine-webassembly-server.sqlite3")}");
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    // Register the entity sets needed by OpenIddict.
    // Note: use the generic overload if you need
    // to replace the default OpenIddict entities.
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapTravelokaAPI();
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapFallbackToFile("/index.html");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
