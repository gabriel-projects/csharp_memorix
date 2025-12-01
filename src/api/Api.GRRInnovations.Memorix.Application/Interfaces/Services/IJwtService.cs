using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Services
{
    public interface IJwtService
    {
        JwtResultModel GenerateToken(IUser user);
        Task<JwtResultModel> GenerateTokenWithRefreshTokenAsync(IUser user);
        Task<JwtResultModel?> RefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        IUser? FromJwt(string token);
    }
}
