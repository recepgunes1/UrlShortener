using ExpiryChecker.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadInfrastructureLayer(builder.Configuration);

builder.Services.AddCors();

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

app.UseRouting();

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

app.Run();
