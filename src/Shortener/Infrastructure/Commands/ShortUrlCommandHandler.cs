using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.ConfigModels;
using System.Text;

namespace Infrastructure.Commands
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

    public class ShortUrlCommand : IRequest<UrlDto>
    {
        public string Url { get; set; }
        public ShortUrlCommand(string url) => Url = url;
    }

    public class ShortUrlCommandHandler : IRequestHandler<ShortUrlCommand, UrlDto>
    {
        private readonly IConfiguration configuration;
        private readonly AppDbContext appDbContext;

        public ShortUrlCommandHandler(IConfiguration _configuration, AppDbContext _appDbContext)
        {
            configuration = _configuration;
            appDbContext = _appDbContext;
        }

        public async Task<UrlDto> Handle(ShortUrlCommand request, CancellationToken cancellationToken)
        {
            var shortenerConfig = configuration.GetSection("Shortener").Get<ShortenerConfig>() ??
                 throw new ArgumentException("Shortener section is broken in appsettings file.");
            var shortedPathBuilder = new StringBuilder();
            var random = new Random();

            for (var i = 0; i < shortenerConfig.Length; i++)
            {
                var index = random.Next(shortenerConfig.CharSet.Length);
                shortedPathBuilder.Append(shortenerConfig.CharSet[index]);
            }

            var url = await appDbContext.Urls.FirstOrDefaultAsync(p => p.LongUrl == request.Url);
            if (url != null)
            {
                url.ShortPath = shortedPathBuilder.ToString();
            }
            else
            {
                appDbContext.Urls.Add(new Shared.Entities.Url { Id = Guid.NewGuid(), LongUrl = request.Url, ShortPath = shortedPathBuilder.ToString(), IsPublic = true});
            }

            await appDbContext.SaveChangesAsync();

            return new();
        }
    }
}
