using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.Models;
using Api.GRRInnovations.Memorix.Infrastructure.Security.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<JwtService> _logger;

        public JwtService(
            IOptions<JwtSettings> jwtSettings,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            ILogger<JwtService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
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

        public async Task<JwtResultModel> GenerateTokenWithRefreshTokenAsync(IUser user)
        {
            var jwtResult = GenerateToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user);
            
            jwtResult.RefreshToken = refreshToken.Token;
            
            await _unitOfWork.SaveChangesAsync();
            
            return jwtResult;
        }

        public async Task<JwtResultModel?> RefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            
            if (token == null || !await _refreshTokenRepository.IsTokenValidAsync(refreshToken))
            {
                return null;
            }

            await _refreshTokenRepository.RevokeAsync(token.Uid);
            
            if (token.DbUser == null)
            {
                return null;
            }

            var newTokens = await GenerateTokenWithRefreshTokenAsync(token.DbUser);
            
            return newTokens;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            
            if (token != null)
            {
                await _refreshTokenRepository.RevokeAsync(token.Uid);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private async Task<RefreshToken> GenerateRefreshTokenAsync(IUser user)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            
            var refreshTokenValue = Convert.ToBase64String(randomNumber);
            
            var expirationDays = _jwtSettings.RefreshTokenExpirationDays > 0
                ? _jwtSettings.RefreshTokenExpirationDays
                : 30;
            
            var refreshToken = new RefreshToken
            {
                UserUid = user.Uid,
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(expirationDays),
                IsRevoked = false
            };

            await _refreshTokenRepository.CreateAsync(refreshToken);
            
            return refreshToken;
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
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "Invalid JWT token. Token preview: {TokenPreview}", 
                    !string.IsNullOrEmpty(token) && token.Length > 20 ? token.Substring(0, 20) : "N/A");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error validating JWT token");
                return null;
            }

            return result;
        }
    }
}
