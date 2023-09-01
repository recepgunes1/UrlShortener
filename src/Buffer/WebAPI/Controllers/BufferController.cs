using Buffer.Infrastructure.Queries;
using Logger.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Buffer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ActionLogFilter))]
    [ServiceFilter(typeof(ErrorLogFilter))]
    public class BufferController : ControllerBase
    {
        private readonly IMediator mediator;

        public BufferController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpGet]
        [Route("get_url/{url}")]
        public async Task<IActionResult> GetUrl(string url)
        {
            url = WebUtility.UrlDecode(url);
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
