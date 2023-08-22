using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadInfrastructreLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseRouting();

app.MapControllers();

app.Run();
