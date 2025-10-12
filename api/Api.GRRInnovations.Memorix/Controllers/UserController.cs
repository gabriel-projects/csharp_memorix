using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.GRRInnovations.Memorix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet()]
        public async Task<IActionResult> Users()
        {
            return Ok(new
            {
                teste = "ga"
            });
        }
    }
}
