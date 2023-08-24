using Microsoft.AspNetCore.Http;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

namespace Gateway.Infrastructure.Aggregators
{
    public class BufferAndShortenerAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            throw new NotImplementedException();
        }
    }
}
