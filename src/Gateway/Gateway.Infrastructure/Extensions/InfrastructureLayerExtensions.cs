using Gateway.Infrastructure.Aggregators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Multiplexer;


namespace Gateway.Infrastructure.Extensions
{
    public static class InfrastructureLayerExtensions
    {
        public static IServiceCollection LoadInfrastructreLayer(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddSingleton<IDefinedAggregator, BufferAndShortenerAggregator>();
            service.AddOcelot();
            return service;
        }
    }
}