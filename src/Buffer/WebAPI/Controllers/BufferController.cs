using Buffer.Infrastructure.Queries;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Extensions;
using System.Net;

namespace Buffer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BufferController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ILogger logger;

        public BufferController(IMediator _mediator, ILogger<BufferController> _logger, IPublishEndpoint _publishEndpoint)
        {
            mediator = _mediator;
            logger = _logger;
            publishEndpoint = _publishEndpoint;
        }

        [HttpGet]
        [Route("/get_url/{url}")]
        public async Task<IActionResult> GetUrl(string url)
        {
            url = WebUtility.UrlDecode(url);
            logger.LogInformation($"{{{nameof(url)}: {url}}} information was send to {nameof(GetUrlFromBufferQuery)}");
            var result = await mediator.Send(new GetUrlFromBufferQuery(url));
            return Ok(result);
        }

        [HttpGet]
        [Route("/publish_url_to_shortener/{url}/{expireDate:int}/{isPublic:bool}")]
        public async Task<IActionResult> PublishUrlToCreateShortPath(string url, int expireDate, bool isPublic)
        {
            try
            {
                url = WebUtility.UrlDecode(url);
                await publishEndpoint.Publish(new ShortedUrlDto { Url = url, ExpireDateTime = DateTime.UtcNow.AddSeconds(expireDate), IsPublic = isPublic });
                logger.LogInformation($"{url} was published as {nameof(ShortedUrlDto)}");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex.ToJsonString());
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/get_all_urls")]
        public async Task<IActionResult> GetAllUrls()
        {
            var result = await mediator.Send(new GetAllUrlsFromBufferQuery());
            return Ok(result?.Urls);
        }
    }
}
