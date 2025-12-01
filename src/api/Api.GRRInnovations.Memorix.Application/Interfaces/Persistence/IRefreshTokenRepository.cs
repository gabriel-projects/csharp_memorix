using Api.GRRInnovations.Memorix.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Persistence
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
        Task<RefreshToken> CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task RevokeAsync(Guid tokenId, string? replacedByToken = null, CancellationToken cancellationToken = default);
        Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<bool> IsTokenValidAsync(string token, CancellationToken cancellationToken = default);
    }
}

