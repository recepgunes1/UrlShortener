using Dapper;
using MediatR;
using System.Data;

namespace Infrastructure.Queries
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
        public GetAllUrlsFromBufferQueryHandler(IDbConnection _connection)
        {
            connection = _connection;
        }

        public async Task<UrlEnumerableDto> Handle(GetAllUrlsFromBufferQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var sql = "SELECT \"Id\", \"LongUrl\", \"ShortPath\", \"CreatedDate\", \"LastRequestedDate\", \"RequestCounter\", \"ExpireDate\", \"IsPublic\" " +
                    "FROM public.\"Urls\" " +
                    "WHERE  \"IsPublic\" = true;";
                var result = await connection.QueryAsync<UrlDto>(sql);
                ArgumentNullException.ThrowIfNull(result);
                return new() { Urls = result };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new();
            }
        }
    }
}
