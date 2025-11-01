using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Application.Wrappers.In;
using Api.GRRInnovations.Memorix.Application.Wrappers.Out;
using Api.GRRInnovations.Memorix.Application.Wrappers;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] WrapperInLogin wrapperInLogin)
        {
            var user = await _authService.ValidateAsync(wrapperInLogin.Email, wrapperInLogin.Password);
            if (user == null) return Unauthorized(Result<string>.Fail("Invalid credentials."));

            var jwt = await _jwtService.GenerateTokenWithRefreshTokenAsync(user);

            var response = await WrapperOutJwtResult.From(jwt).ConfigureAwait(false);
            return new OkObjectResult(Result<WrapperOutJwtResult>.Ok(response));
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] WrapperInRefreshToken request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(Result<string>.Fail("Refresh token is required."));

            var jwt = await _jwtService.RefreshTokenAsync(request.RefreshToken);
            
            if (jwt == null)
                return Unauthorized(Result<string>.Fail("Invalid or expired refresh token."));

            var response = await WrapperOutJwtResult.From(jwt).ConfigureAwait(false);
            return new OkObjectResult(Result<WrapperOutJwtResult>.Ok(response));
        }

        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] WrapperInRefreshToken request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(Result<string>.Fail("Refresh token is required."));

            await _jwtService.RevokeRefreshTokenAsync(request.RefreshToken);
            
            return Ok(Result<string>.Ok("Token revoked successfully."));
        }
    }
}
