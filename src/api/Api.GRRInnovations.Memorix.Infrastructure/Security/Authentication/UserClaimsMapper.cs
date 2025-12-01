using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Security.Authentication
{
    public class UserClaimsMapper
    {
        public UserClaimsModel MapFromClaimsPrincipal(ClaimsPrincipal principal)
        {
            var identity = principal?.Identities?.FirstOrDefault();
            if (identity == null) return null;

            string firstName = identity.FindFirst(ClaimTypes.GivenName)?.Value;
            string lastName = identity.FindFirst(ClaimTypes.Surname)?.Value;
            string fullName = identity.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                if (!string.IsNullOrEmpty(fullName))
                {
                    var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    firstName = parts.FirstOrDefault() ?? fullName;
                    lastName = parts.Length > 1 ? parts[^1] : "N/A";
                }
            }

            return new UserClaimsModel
            {
                Email = identity.FindFirst(ClaimTypes.Email)?.Value,
                Name = fullName,
                FirstName = firstName,
                LastName = lastName,
                PrivateId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
            };
        }
    }

    public class UserClaimsModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrivateId { get; set; }
        public string Name { get; set; }
    }
}
