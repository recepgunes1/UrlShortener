using System.Collections;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Shared.Extensions
{
    public static class SharedExtensions
    {
        public static string ToJsonString(this Exception ex)
        {
            var exceptionDetails = new
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Source = ex.Source,
                HelpLink = ex.HelpLink,
                Data = ex.Data.Cast<DictionaryEntry>().ToDictionary(entry => entry.Key?.ToString() ?? Guid.NewGuid().ToString(), entry => entry.Value?.ToString()),
                InnerException = ex.InnerException != null ? new
                {
                    Message = ex.InnerException.Message,
                    StackTrace = ex.InnerException.StackTrace,
                    Source = ex.InnerException.Source
                } : null
            };

            var jsonException = JsonSerializer.Serialize(exceptionDetails, new JsonSerializerOptions { WriteIndented = true });
            return jsonException;
        }
        
        public static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }
    }
}
