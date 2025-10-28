using Api.GRRInnovations.Memorix.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Security
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _accessor;

        public UserContext(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid? UserId
        {
            get
            {
                var id = _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Guid.TryParse(id, out var guid) ? guid : null;
            }
        }

        public string? Email => _accessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        public string? Name => _accessor.HttpContext?.User?.Identity?.Name;
        public bool IsAuthenticated => _accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
