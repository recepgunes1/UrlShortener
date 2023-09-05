using Logger.Filters;
using Logger.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Logger.Extensions
{
    public static class LoggerExtensions
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticConfiguration:Uri"] ?? throw new ArgumentNullException();

            var settings = new ConnectionSettings(new Uri(url))
                .EnableDebugMode()
                .PrettyJson()
                .RequestTimeout(TimeSpan.FromMinutes(2))
                .EnableDebugMode();

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            services.AddScoped<IElasticsearchService, ElasticsearchService>();

            services.AddScoped<ActionLogFilter>();
            services.AddScoped<ErrorLogFilter>();

            return services;
        }

    }
}
