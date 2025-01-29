using Infrastructure.Database;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.InitLogger(true);

var dbres = builder.AddDatabase();
if (dbres.IsFailed) {
    Console.WriteLine("CRITICAL ERROR! Could not add Database to builder!\n" +
        "The following errors were encountered:");
    dbres.Errors.ForEach(err => Console.WriteLine($"- {err.Message}"));
    throw new Exception("CRITICAL ERROR! Could not add Database to builder!");
}

var app = builder.Build();

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

dbres = app.InitializeDatabase();
if (dbres.IsFailed) {
    app.GetLogger().Error($"DB initilization failed:");
    dbres.Errors.ForEach(err => app.GetLogger().Error($"- {err.Message}"));
    throw new Exception("CRITICAL ERROR: DB Initialization failed!");
}

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
