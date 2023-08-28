using Buffer.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Buffer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BufferController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public BufferController(IMediator _mediator, ILogger<BufferController> _logger)
        {
            mediator = _mediator;
            logger = _logger;
        }

        [HttpGet]
        [Route("get_url/{url}")]
        public async Task<IActionResult> GetUrl(string url)
        {
            url = WebUtility.UrlDecode(url);
            logger.LogInformation($"{{{nameof(url)}: {url}}} information was send to {nameof(GetUrlFromBufferQuery)}");
            var result = await mediator.Send(new GetUrlFromBufferQuery(url));
            return Ok(result);
        }

        [HttpGet]
        [Route("get_all_urls")]
        public async Task<IActionResult> GetAllUrls()
        {
            var result = await mediator.Send(new GetAllUrlsFromBufferQuery());
            return Ok(result?.Urls);
        }
    }
}
