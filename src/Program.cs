using FluentResults;
using Infrastructure;
using Infrastructure.Database;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using System.Text.Json;
using API.EndpointManager;

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
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// Adding all endpoints described in EndpointManager.cs
dbres = app.AddEndpoints();

if (dbres.IsFailed) {
    app.GetLogger().Error("Endpoint initialization failed:");
    dbres.LogAllErrors(app.GetLogger());
    throw new Exception("CRITICAL ERROR: Endpoint Initialization failed!");
}

dbres = app.InitializeDatabase();
if (dbres.IsFailed) {
    app.GetLogger().Error($"DB initilization failed:");
    dbres.LogAllErrors(app.GetLogger());
    throw new Exception("CRITICAL ERROR: DB Initialization failed!");
}

app.Run();
