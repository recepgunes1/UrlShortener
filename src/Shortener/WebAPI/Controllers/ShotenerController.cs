using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shortener.Infrastructure.Queries;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShotenerController : ControllerBase
    {
        private readonly IMediator mediator;

        public ShotenerController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpGet]
        [Route("/get_url/{url}")]
        public async Task<IActionResult> GetUrl(string url)
        {
            var result = await mediator.Send(new GetUrlQuery(url));
            return Ok(result);
        }
    }
}
