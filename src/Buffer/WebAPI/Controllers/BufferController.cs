using Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BufferController : ControllerBase
    {

        private readonly IMediator _mediator;

        public BufferController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/get_url/{url}")]
        public async Task<IActionResult> GetUrl(string url)
        {
            var test = await _mediator.Send(new GetUrlFromBufferQuery(url));
            return Ok(test);
        }

        [HttpGet]
        [Route("/get_all_urls")]
        public async Task<IActionResult> GetAllUrls()
        {
            var test = await _mediator.Send(new GetAllUrlsFromBufferQuery());
            return Ok(test.Urls);
        }

    }
}
