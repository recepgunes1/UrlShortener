using Shortener.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Services.LoadInfrastructureLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseRouting();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

app.MapControllers();

app.Run();
