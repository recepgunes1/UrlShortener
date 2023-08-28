using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Extensions;
using System.Data;
using System.Net;

namespace Shortener.Infrastructure.Queries
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

    public class GetUrlQuery : IRequest<UrlDto>
    {
        public string Url { get; set; }
        public GetUrlQuery(string url) => Url = url;
    }

    public class GetUrlQueryHandler : IRequestHandler<GetUrlQuery, UrlDto>
    {
        private readonly IDbConnection connection;
        private readonly ILogger logger;

        public GetUrlQueryHandler(IDbConnection _connection, ILogger<GetUrlQueryHandler> _logger)
        {
            connection = _connection;
            logger = _logger;
        }

        public async Task<UrlDto> Handle(GetUrlQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"Request was handled by {nameof(GetUrlQueryHandler)}");
                var decodedUrl = WebUtility.UrlDecode(request.Url);
                var sql = @"SELECT ""Id"", ""LongUrl"", ""ShortPath"", ""CreatedDate"", ""LastRequestedDate"", ""RequestCounter"", ""ExpireDate"", ""IsPublic"" 
FROM public.""Urls"" 
WHERE  ""IsPublic"" = true AND (""LongUrl"" = @url OR ""ShortPath"" = @url);";
                var parameters = new { url = decodedUrl };
                var result = await connection.QueryFirstAsync<UrlDto>(sql, parameters);
                logger.LogInformation($"Executed Query: {sql}{Environment.NewLine}Params: {parameters}");
                ArgumentNullException.ThrowIfNull(result);
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
