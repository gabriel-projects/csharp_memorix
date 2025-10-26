using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Application.Services;
using Api.GRRInnovations.Memorix.Application.Wrappers.Out;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.GRRInnovations.Memorix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var name = User.Identity?.Name; // "gabriel ribeiro"
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var user = await _userService.GetUserByUidAsync(Guid.Parse(id!));
            var response = await WrapperOutUser.From(user);

            return Ok(Result<WrapperOutUser>.Ok(response));
        }
    }
}
