﻿using ExpiryChecker.Infrastructure.Consumers;
using ExpiryChecker.Infrastructure.Context;
using ExpiryChecker.Infrastructure.Jobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Shared.ConfigModels;
using System.Collections.Specialized;

namespace ExpiryChecker.Infrastructure.Extensions
{
    public static class InfrastructureLayerExtensions
    {
        public static IServiceCollection LoadInfrastructreLayer(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<AppDbContext>(p => p.UseNpgsql(configuration.GetConnectionString("defaultForUrlShortener")));
            service.AddQuartz();
            service.AddQuartzHostedService(opt =>
            {
                opt.WaitForJobsToComplete = true;
            });

            service.AddScoped<IScheduler>(_ => new StdSchedulerFactory(new NameValueCollection
            {
                { "quartz.serializer.type", "json" },
                { "quartz.jobStore.clustered", "true" },
                { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                { "quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz" },
                { "quartz.jobStore.tablePrefix", "QRTZ_" },
                { "quartz.jobStore.dataSource", "myDS" },
                { "quartz.dataSource.myDS.connectionString", configuration.GetConnectionString("defaultForQuartz") },
                { "quartz.dataSource.myDS.provider", "MySql" },
                { "quartz.jobStore.useProperties", "true" },
                {"quartz.jobStore.performSchemaValidation", "false" }

            }).GetScheduler().Result);

            service.AddTransient<ExpireUrlJob>();

            service.AddMassTransit(config =>
            {
                config.AddConsumer<ExpiredUrlConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    var rabbitMqCredentials = configuration.GetSection("RabbitMQ").Get<RabbitMqCredentials>() ??
                    throw new ArgumentException("RabbitMQ section is broken in appsettings file.");
                    cfg.Host(rabbitMqCredentials.Host, rabbitMqCredentials.Port, rabbitMqCredentials.VirtualHost, u =>
                    {
                        u.Username(rabbitMqCredentials.Username);
                        u.Password(rabbitMqCredentials.Password);
                    });
                    cfg.ReceiveEndpoint("ExpiredUrlConsumer", e => e.ConfigureConsumer<ExpiredUrlConsumer>(ctx));
                });
            });
            return service;
        }
    }
}