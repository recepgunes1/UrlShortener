using Gateway.Infrastructure.Extensions;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadInfrastructreLayer(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();

app.Run();
