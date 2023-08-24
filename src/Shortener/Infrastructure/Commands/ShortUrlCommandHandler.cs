using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.ConfigModels;
using Shared.DTOs;
using Shortener.Infrastructure.Context;
using System.Text;

namespace Shortener.Infrastructure.Commands
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
        public DateTime ExpireDateTime { get; set; }
        public bool IsPublic { get; set; }
        public ShortUrlCommand(string url, DateTime expireDateTime, bool isPublic)
        {
            Url = url;
            ExpireDateTime = expireDateTime;
            IsPublic = isPublic;
        }
    }

    public class ShortUrlCommandHandler : IRequestHandler<ShortUrlCommand, UrlDto>
    {
        private readonly IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ILogger logger;

        public ShortUrlCommandHandler(IConfiguration _configuration, AppDbContext _appDbContext, IPublishEndpoint _publishEndpoint, ILogger<ShortUrlCommandHandler> _logger)
        {
            configuration = _configuration;
            appDbContext = _appDbContext;
            publishEndpoint = _publishEndpoint;
            logger = _logger;
        }

        public async Task<UrlDto> Handle(ShortUrlCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Request was handled by {nameof(ShortUrlCommandHandler)}");
            var shortenerConfig = configuration.GetSection("Shortener").Get<ShortenerConfig>() ??
                 throw new ArgumentException("Shortener section is broken in appsettings file.");
            var shortedPathBuilder = new StringBuilder();
            var random = new Random();

            for (var i = 0; i < shortenerConfig.Length; i++)
            {
                var index = random.Next(shortenerConfig.CharSet.Length);
                shortedPathBuilder.Append(shortenerConfig.CharSet[index]);
            }

            logger.LogInformation($"Short Path created for {request.Url}");

            var url = await appDbContext.Urls.FirstOrDefaultAsync(p => p.LongUrl == request.Url);
            if (url != null)
            {
                url.ShortPath = shortedPathBuilder.ToString();
                logger.LogInformation($"Short Path assigned for {request.Url}");
            }
            else
            {
                var entity = new Shared.Entities.Url
                {
                    Id = Guid.NewGuid(),
                    LongUrl = request.Url,
                    ShortPath = shortedPathBuilder.ToString(),
                    CreatedDate = DateTime.UtcNow,
                    RequestCounter = 0,
                    LastRequestedDate = null,
                    ExpireDate = request.ExpireDateTime,
                    IsPublic = request.IsPublic
                };
                appDbContext.Urls.Add(entity);
                logger.LogInformation($"New entity created <{entity}>");
                await publishEndpoint.Publish(new ExpiredUrlDto { LongUrl = request.Url, ExpireDateTime = request.ExpireDateTime });
                logger.LogInformation($"{request.Url} was published as {nameof(ExpiredUrlDto)}");
            }

            await appDbContext.SaveChangesAsync();

            logger.LogInformation($"Changes commited for {request.Url}");

            return new();
        }
    }
}
