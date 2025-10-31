using Api.GRRInnovations.Memorix.Application.Interfaces;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Application.Wrappers.Out;
using Api.GRRInnovations.Memorix.Domain.Models;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.GRRInnovations.Memorix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserContext _userContext;

        public UserController(IUserService userService, 
            IUserContext userContext)
        {
            _userService = userService;
            _userContext = userContext;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _userContext.RequireUserId();

            var user = await _userService.GetUserByUidAsync(userId);
            if (user == null)
                return NotFound(Result<string>.Fail("User not found."));

            var response = await WrapperOutUser.From(user);
            return Ok(Result<WrapperOutUser>.Ok(response));
        }

        [HttpGet("full")]
        [Authorize]
        public async Task<IActionResult> Full()
        {
            var userId = _userContext.RequireUserId();

            var options = UserOptions.Create()
                .IncludeCards()
                .Build();

            var user = await _userService.GetUserByUidAsync(userId, options);
            if (user == null)
                return NotFound(Result<string>.Fail("User not found."));

            var response = await WrapperOutUserFull.From(user);
            return Ok(Result<WrapperOutUserFull>.Ok(response));
        }
    }
}
