using Gateway.Infrastructure.Extensions;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadInfrastructreLayer(builder.Configuration);

builder.Services.AddCors();

var app = builder.Build();

app.Use((context, next) =>
{
    context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE";
    context.Response.Headers["Access-Control-Allow-Headers"] = "*";
    return next.Invoke();
});

app.UseCors(p =>
{
    p.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

await app.UseOcelot();

app.Run();
