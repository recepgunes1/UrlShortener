using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Extensions;
using System.Data;

namespace Buffer.Infrastructure.Queries
{
    public record UrlEnumerableDto
    {
        public IEnumerable<UrlDto> Urls { get; set; } = default!;
    }

    public class GetAllUrlsFromBufferQuery : IRequest<UrlEnumerableDto>
    {
    }

    public class GetAllUrlsFromBufferQueryHandler : IRequestHandler<GetAllUrlsFromBufferQuery, UrlEnumerableDto>
    {
        private readonly IDbConnection connection;
        private readonly ILogger logger;

        public GetAllUrlsFromBufferQueryHandler(IDbConnection _connection, ILogger<GetAllUrlsFromBufferQueryHandler> _logger)
        {
            connection = _connection;
            logger = _logger;
        }

        public async Task<UrlEnumerableDto> Handle(GetAllUrlsFromBufferQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"Request was handled by {nameof(GetAllUrlsFromBufferQueryHandler)}");
                var sql = @"SELECT ""Id"", ""LongUrl"", ""ShortPath"", ""CreatedDate"", ""LastRequestedDate"", ""RequestCounter"", ""ExpireDate"", ""IsPublic"" 
FROM public.""Urls"" 
WHERE  ""IsPublic"" = true;";
                var result = await connection.QueryAsync<UrlDto>(sql);
                logger.LogInformation($"Executed Query:{Environment.NewLine}\t{sql}");
                ArgumentNullException.ThrowIfNull(result);
                return new() { Urls = result };
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception Thrown:{Environment.NewLine}{ex.ToJsonString()}");
                return new();
            }
        }
    }
}
