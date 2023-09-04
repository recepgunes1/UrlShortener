using Gateway.Infrastructure.Extensions;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Services.LoadInfrastructreLayer(builder.Configuration);

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(p =>
{
    p.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

await app.UseOcelot();

app.Run();
