using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Application.Services;
using Api.GRRInnovations.Memorix.Application.Wrappers.In;
using Api.GRRInnovations.Memorix.Application.Wrappers.Out;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Models;
using Api.GRRInnovations.Memorix.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.GRRInnovations.Memorix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
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
        public async Task<IActionResult> Login([FromBody] WrapperInLogin<User> wrapperInLogin)
        {
            var wrapperModel = await wrapperInLogin.Result();
            if (wrapperModel == null)
                return BadRequest(Result<string>.Fail("Invalid input data."));

            var remoteUser = await _authService.ValidateAsync(wrapperModel);
            if (remoteUser == null) return new UnauthorizedResult();

            var userToken = _jwtService.GenerateToken(remoteUser);

            var response = await WrapperOutJwtResult.From(userToken).ConfigureAwait(false);
            return new OkObjectResult(Result<WrapperOutJwtResult>.Ok(response));
        }
    }
}
