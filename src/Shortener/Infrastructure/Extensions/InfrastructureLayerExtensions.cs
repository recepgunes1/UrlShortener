﻿using Logger.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shared.ConfigModels;
using Shortener.Infrastructure.Consumers;
using Shortener.Infrastructure.Context;
using Shortener.Infrastructure.Services;
using System.Data;
using System.Reflection;

namespace Shortener.Infrastructure.Extensions
{
    public static class InfrastructureLayerExtensions
    {
        public static IServiceCollection LoadInfrastructureLayer(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddMediatR(p =>
            {
                p.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            service.AddScoped<IDbConnection>(_ => new NpgsqlConnection(configuration.GetConnectionString("default")));

            service.AddDbContext<AppDbContext>(p => p.UseNpgsql(configuration.GetConnectionString("default")));

            service.AddMassTransit(config =>
            {
                config.AddConsumer<ShortedUrlConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    var rabbitMqCredentials = configuration.GetSection("RabbitMQ").Get<RabbitMqCredentials>() ??
                    throw new ArgumentException("RabbitMQ section is broken in appsettings file.");
                    cfg.Host(rabbitMqCredentials.Host, rabbitMqCredentials.Port, rabbitMqCredentials.VirtualHost, u =>
                    {
                        u.Username(rabbitMqCredentials.Username);
                        u.Password(rabbitMqCredentials.Password);
                    });
                    cfg.ReceiveEndpoint("shorten_url_service", e => e.ConfigureConsumer<ShortedUrlConsumer>(ctx));
                });
            });

            service.AddHostedService<OutboxWorker>();

            service.AddElasticSearch(configuration);

            return service;
        }
    }
}
