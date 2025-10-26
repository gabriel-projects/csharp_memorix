using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Application.Wrappers.In;
using Api.GRRInnovations.Memorix.Application.Wrappers.Out;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.Models;
using Api.GRRInnovations.Memorix.Filters;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.GRRInnovations.Memorix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] WrapperInRegister<User> wrapperInRegister)
        {
            var wrapperModel = await wrapperInRegister.Result();
            if (wrapperModel == null)
                return BadRequest(Result<string>.Fail("Invalid input data."));

            var user = await _authService.RegisterAsync(wrapperModel);

            var response = await WrapperOutUser.From(user);

            return Ok(Result<WrapperOutUser>.Ok(response));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            // Login logic here
            return Ok();
        }


    }
}
