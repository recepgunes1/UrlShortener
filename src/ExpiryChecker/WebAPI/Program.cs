using ExpiryChecker.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Services.LoadInfrastructureLayer(builder.Configuration);

var app = builder.Build();

app.UseRouting();

app.Run();
