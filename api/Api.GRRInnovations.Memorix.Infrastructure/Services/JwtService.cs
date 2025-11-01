using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.Models;
using Api.GRRInnovations.Memorix.Infrastructure.Security.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public JwtResultModel GenerateToken(IUser user)
        {
            var expirationMinutes = _jwtSettings.ExpirationDays > 0 
                ? _jwtSettings.ExpirationDays 
                : 10; 
            var expireDiff = TimeSpan.FromDays(expirationMinutes);

            var jwtData = new JwtModel(user);

            if (jwtData.NotBefore == null) jwtData.NotBefore = DateTime.UtcNow;
            jwtData.ExpireAt = jwtData.NotBefore + expireDiff;

            var jwtKey = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var securityKey = new SymmetricSecurityKey(jwtKey);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = JwtClaimHelper.GenerateClaims(user);
            var identity = new ClaimsIdentity(claims, "OAuth");

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = credentials,
                IssuedAt = jwtData.NotBefore,
                Subject = identity,
                NotBefore = jwtData.NotBefore,
                Expires = jwtData.ExpireAt
            });

            var token = handler.WriteToken(securityToken);

            var result = new JwtResultModel
            {
                AccessToken = token,
                Expire = new DateTimeOffset(jwtData.ExpireAt.Value).ToUnixTimeSeconds(),
                Type = "Bearer"
            }; ;

            return result;
        }

        public IUser? FromJwt(string token)
        {
            var info = Validate(token);
            if (info == null) return null;

            var result = new JwtModel(info.Claims.ToList())
            {
                Jwt = token,
                JwtToken = info,
                NotBefore = info.ValidFrom,
                ExpireAt = info.ValidTo
            };

            return result.Model;
        }

        private JwtSecurityToken Validate(string token)
        {
            JwtSecurityToken result = null;
            try
            {
                var jwtKey = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
                var tokenHandler = new JwtSecurityTokenHandler();
                var paramsValidation = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
                    ValidAudience = _jwtSettings.Audience,
                    ValidIssuer = _jwtSettings.Issuer
                };

                SecurityToken securityToken;
                tokenHandler.ValidateToken(token, paramsValidation, out securityToken);

                result = tokenHandler.ReadJwtToken(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return result;
        }
    }
}
