using Dapper;
using MassTransit;
using MediatR;
using Shared.DTOs;
using System.Data;
using System.Net;

namespace Infrastructure.Queries
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
        private readonly IPublishEndpoint publishEndpoint;

        public GetUrlFromBufferQueryHandler(IDbConnection _connection, IPublishEndpoint _publishEndpoint)
        {
            connection = _connection;
            publishEndpoint = _publishEndpoint;
        }

        public async Task<UrlDto> Handle(GetUrlFromBufferQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var decodedUrl = WebUtility.UrlDecode(request.Url);
                var sql = "SELECT \"Id\", \"LongUrl\", \"ShortPath\", \"CreatedDate\", \"LastRequestedDate\", \"RequestCounter\", \"ExpireDate\", \"IsPublic\" " +
                    "FROM public.\"Urls\" " +
                    "WHERE  \"IsPublic\" = true AND (\"LongUrl\" = @url OR \"ShortPath\" = @url);";
                var parameters = new { url = decodedUrl };
                var result = await connection.QueryFirstOrDefaultAsync<UrlDto>(sql, parameters);
                if (result == null || string.IsNullOrEmpty(result.ShortPath))
                {
                    await publishEndpoint.Publish(new ShortedUrlDto { Url = decodedUrl });
                    return new();
                }
                ArgumentNullException.ThrowIfNull(result);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new();
            }
        }
    }
}
