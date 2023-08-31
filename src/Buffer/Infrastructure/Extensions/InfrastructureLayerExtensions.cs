using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shared.ConfigModels;
using System.Data;
using System.Reflection;

namespace Buffer.Infrastructure.Extensions
{
    public static class InfrastructureLayerExtensions
    {
        public static IServiceCollection LoadInfrastructureLayer(this IServiceCollection service, IConfiguration configuration)
        {

            service.AddMediatR(p => p.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            service.AddScoped<IDbConnection>(_ => new NpgsqlConnection(configuration.GetConnectionString("default")));
            service.AddMassTransit(x => x.UsingRabbitMq((ctx, cfg) =>
            {
                var rabbitMqConfig = configuration.GetSection("RabbitMQ").Get<RabbitMqCredentials>() ??
                             throw new ArgumentException("RabbitMQ section is broken in appsettings file.");
                cfg.Host(rabbitMqConfig.Host, rabbitMqConfig.Port, rabbitMqConfig.VirtualHost, u =>
                {
                    u.Username(rabbitMqConfig.Username);
                    u.Password(rabbitMqConfig.Password);
                });
                cfg.ConfigureEndpoints(ctx);
            }));
            return service;
        }
    }
}
