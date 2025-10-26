using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Security.Authentication
{
    public static class JwtClaimHelper
    {
        public const string ClaimUserUid = "user_uid";
        private const string ClaimEmail = "email";

        public static List<Claim> GenerateClaims(IUser model)
        {
            var claims = new List<Claim>();

            if (model == null) return claims;

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                claims.Add(new Claim(ClaimEmail, model.Email));
            }

            claims.Add(new Claim(ClaimUserUid, model.Uid.ToString()));

            return claims;
        }

        public static IUser ExtractUserFromClaims(List<Claim> claims)
        {
            if (claims == null || claims.Count == 0)
                return null;

            var userUidClaim = claims.FirstOrDefault(c => c.Type == ClaimUserUid)?.Value;
            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userUidClaim))
                return null; // Retorna nulo se não encontrar o UID

            return new User
            {
                Uid = Guid.Parse(userUidClaim),
                Email = new Domain.ValueObjects.Email(emailClaim)
            };
        }
    }
}
