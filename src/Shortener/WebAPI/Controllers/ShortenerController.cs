using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Extensions;
using Shortener.Infrastructure.Context;
using Shortener.Infrastructure.Models;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortenerController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly AppDbContext appDbContext;

        public ShortenerController(ILogger<ShortenerController> _logger, AppDbContext _appDbContext)
        {
            logger = _logger;
            appDbContext = _appDbContext;
        }

        [HttpPost]
        [Route("publish_url")]
        public async Task<IActionResult> PublishUrlToCreateShortPath([FromBody] ShortUrlRequest request)
        {
            try
            {
                var dto = new ShortedUrlDto
                {
                    Url = request.Url,
                    ExpireDateTime = DateTime.UtcNow.AddSeconds(request.ExpireDate),
                    IsPublic = request.IsPublic
                };
                var outboxMessage = new OutboxMessage()
                {
                    EventPayload = JsonSerializer.Serialize(dto),
                    EventType = dto.GetType().AssemblyQualifiedName!
                };
                await appDbContext.OutboxMessages.AddAsync(outboxMessage);
                await appDbContext.SaveChangesAsync();
                logger.LogInformation($"{request.Url} was saved in outbox table. Outbox ID: {outboxMessage.Id}");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex.ToJsonString());
                return BadRequest();
            }
        }
    }
}
