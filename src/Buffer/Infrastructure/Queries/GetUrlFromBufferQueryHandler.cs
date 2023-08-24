using Dapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Extensions;
using System.Data;
using System.Net;

namespace Buffer.Infrastructure.Queries
{
    public record UrlDto
    {
        public string LongUrl { get; set; } = null!;
        public string? ShortPath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastRequestedDate { get; set; }
        public int RequestCounter { get; set; }
        public DateTime? ExpireDate { get; set; }
    }

    public class GetUrlFromBufferQuery : IRequest<UrlDto>
    {
        public string Url { get; set; } = null!;

        public GetUrlFromBufferQuery(string _url)
        {
            Url = _url;
        }
    }

    public class GetUrlFromBufferQueryHandler : IRequestHandler<GetUrlFromBufferQuery, UrlDto>
    {
        private readonly IDbConnection connection;
        private readonly ILogger logger;

        public GetUrlFromBufferQueryHandler(IDbConnection _connection, ILogger<GetUrlFromBufferQueryHandler> _logger)
        {
            connection = _connection;
            logger = _logger;
        }

        public async Task<UrlDto> Handle(GetUrlFromBufferQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"Request was handled by {nameof(GetUrlFromBufferQueryHandler)}");
                var decodedUrl = WebUtility.UrlDecode(request.Url);
                var sql = @"SELECT ""Id"", ""LongUrl"", ""ShortPath"", ""CreatedDate"", ""LastRequestedDate"", ""RequestCounter"", ""ExpireDate"", ""IsPublic"" 
FROM public.""Urls"" 
WHERE  ""IsPublic"" = true AND (""LongUrl"" = @url OR ""ShortPath"" = @url);";
                var parameters = new { url = decodedUrl };
                var result = await connection.QueryFirstOrDefaultAsync<UrlDto>(sql, parameters);
                logger.LogInformation($"Executed Query: {sql}{Environment.NewLine}Params: {parameters}");
                if (result == null || string.IsNullOrEmpty(result.ShortPath))
                {
                    return new();
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception Thrown:{Environment.NewLine}{ex.ToJsonString()}");
                return new();
            }
        }
    }
}
