using Logger.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shortener.Infrastructure.Commands;
using Shortener.Infrastructure.Context;
using Shortener.Infrastructure.Models;
using System.Text.Json;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ActionLogFilter))]
    [ServiceFilter(typeof(ErrorLogFilter))]
    public class ShortenerController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly IMediator mediator;

        public ShortenerController(AppDbContext _appDbContext, IMediator _mediator)
        {
            appDbContext = _appDbContext;
            mediator = _mediator;
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
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("redirect/{shortPath}")]
        public async Task<IActionResult> ReachUrl(string shortPath)
        {
            return Ok(await mediator.Send(new ReachUrlCommand(shortPath)));
        }

    }
}
