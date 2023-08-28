using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.ConfigModels;
using Shared.DTOs;
using Shortener.Infrastructure.Context;
using Shortener.Infrastructure.Models;
using System.Text;
using System.Text.Json;

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
        private readonly ILogger logger;

        public ShortUrlCommandHandler(IConfiguration _configuration, AppDbContext _appDbContext, ILogger<ShortUrlCommandHandler> _logger)
        {
            configuration = _configuration;
            appDbContext = _appDbContext;
            logger = _logger;
        }

        public async Task<UrlDto> Handle(ShortUrlCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Request was handled by {nameof(ShortUrlCommandHandler)}");

            var url = await appDbContext.Urls.FirstOrDefaultAsync(p => p.LongUrl == request.Url);
            var section = configuration.GetSection("Shortener");
            var shortenerConfig = new ShortenerConfig
            {
                CharSet = section["CharSet"]!,
                Length = Convert.ToInt32(section["Length"]!)
            };

            if (shortenerConfig.CharSet == null || shortenerConfig.Length == 0)
            {
                throw new ArgumentException("Shortener section is broken in appsettings file.");
            }
            var shortedPathBuilder = new StringBuilder();
            var random = new Random();

            for (var i = 0; i < shortenerConfig.Length; i++)
            {
                var index = random.Next(shortenerConfig.CharSet.Length);
                shortedPathBuilder.Append(shortenerConfig.CharSet[index]);
            }
            logger.LogInformation($"Short Path created for {request.Url}");

            if (url != null)
            {
                if (url.ShortPath != null)
                {
                    logger.LogWarning($"{request.Url} has shorten path already.");

                    return new();
                }
                url.ShortPath = shortedPathBuilder.ToString();
                url.ExpireDate = request.ExpireDateTime;
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
            }
            var message = new ExpiredUrlDto { LongUrl = request.Url, ExpireDateTime = request.ExpireDateTime };

            var outboxMessage = new OutboxMessage()
            {
                EventPayload = JsonSerializer.Serialize(message),
                EventType = message.GetType().AssemblyQualifiedName!
            };

            await appDbContext.OutboxMessages.AddAsync(outboxMessage);

            logger.LogInformation($"{request.Url} was saved in outbox table. Outbox ID: {outboxMessage.Id}");

            await appDbContext.SaveChangesAsync();

            logger.LogInformation($"Changes commited for {request.Url}");

            return new();
        }
    }
}
