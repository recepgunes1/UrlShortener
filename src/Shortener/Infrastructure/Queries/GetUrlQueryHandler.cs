using Dapper;
using MassTransit;
using MediatR;
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

    public class GetUrlQuery : IRequest<UrlDto>
    {
        public string Url { get; set; }
        public GetUrlQuery(string url) => Url = url;
    }

    public class GetUrlQueryHandler : IRequestHandler<GetUrlQuery, UrlDto>
    {
        private readonly IDbConnection connection;
        private readonly IPublishEndpoint publishEndpoint;

        public GetUrlQueryHandler(IDbConnection _connection, IPublishEndpoint _publishEndpoint)
        {
            connection = _connection;
            publishEndpoint = _publishEndpoint;
        }

        public async Task<UrlDto> Handle(GetUrlQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var decodedUrl = WebUtility.UrlDecode(request.Url);
                var sql = "SELECT \"Id\", \"LongUrl\", \"ShortPath\", \"CreatedDate\", \"LastRequestedDate\", \"RequestCounter\", \"ExpireDate\", \"IsPublic\" " +
                    "FROM public.\"Urls\" " +
                    "WHERE  \"IsPublic\" = true AND (\"LongUrl\" = @url OR \"ShortPath\" = @url);";
                Console.WriteLine(sql);
                var parameters = new { url = decodedUrl };
                var result = await connection.QueryFirstAsync<UrlDto>(sql, parameters);
                ArgumentNullException.ThrowIfNull(result);
                if (string.IsNullOrEmpty(result.ShortPath))
                {
                    //await publishEndpoint.Publish(new ShortedUrlDto(decodedUrl));
                    //publish expire date checker
                    return new();
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                return new();
            }
        }
    }
}
