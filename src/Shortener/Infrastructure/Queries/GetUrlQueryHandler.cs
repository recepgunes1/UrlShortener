using Dapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System.Data;
using System.Net;
using System.Text.Json;

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
        private readonly IPublishEndpoint publishEndpoint;
        public GetUrlQueryHandler(IDbConnection _connection, ILogger<GetUrlQueryHandler> _logger, IPublishEndpoint _publishEndpoint)
        {
            connection = _connection;
            logger = _logger;
            publishEndpoint = _publishEndpoint;
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
                if (string.IsNullOrEmpty(result.ShortPath))
                {
                    //publish shortener  consumer again
                    await publishEndpoint.Publish(new ShortedUrlDto() { Url = result.LongUrl, ExpireDateTime = DateTime.UtcNow.AddSeconds(30), IsPublic = true });
                    return new();
                }
                return result;
            }
            catch (Exception ex)
            {
                var jsonException = JsonSerializer.Serialize(ex, new JsonSerializerOptions { WriteIndented = true });
                logger.LogError($"Exception Thrown:{Environment.NewLine}{jsonException}");
                return new();
            }
        }
    }
}
