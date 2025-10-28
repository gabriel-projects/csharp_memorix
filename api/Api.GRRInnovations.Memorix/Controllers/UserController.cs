using Api.GRRInnovations.Memorix.Application.Interfaces;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Application.Services;
using Api.GRRInnovations.Memorix.Application.Wrappers.Out;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Models;
using Api.GRRInnovations.Memorix.Infrastructure.Security;
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
            if (!_userContext.IsAuthenticated || _userContext.UserId is null)
                return Unauthorized(Result<string>.Fail("User not authenticated."));

            var user = await _userService.GetUserByUidAsync(_userContext.UserId.Value);
            if (user == null)
                return NotFound(Result<string>.Fail("User not found."));

            var response = await WrapperOutUser.From(user);
            return Ok(Result<WrapperOutUser>.Ok(response));
        }
    }
}
