using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Security.Authentication
{
    public class JwtModel
    {
        public JwtModel(IUser model)
        {
            Model = model;
            AdditionalClaims = JwtClaimHelper.GenerateClaims(model);
        }

        public JwtModel(List<Claim> claims)
        {
            AdditionalClaims = claims ?? new List<Claim>();
            Model = JwtClaimHelper.ExtractUserFromClaims(claims);
        }

        public JwtSecurityToken JwtToken { get; set; }

        public List<Claim> AdditionalClaims { get; set; } = new List<Claim>();

        public IUser Model { get; set; }

        public string Jwt { get; set; }

        public DateTime? NotBefore { get; set; }

        public DateTime? ExpireAt { get; set; }
    }
}
