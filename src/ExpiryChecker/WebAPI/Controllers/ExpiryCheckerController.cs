using Microsoft.AspNetCore.Mvc;

namespace ExpiryChecker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpiryCheckerController : ControllerBase
    {
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
