using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RefreshTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RefreshTokens
                .AsNoTracking()
                .Include(rt => rt.DbUser)
                .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            return refreshToken;
        }

        public async Task RevokeAsync(Guid tokenId, string? replacedByToken = null, CancellationToken cancellationToken = default)
        {
            var token = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Uid == tokenId, cancellationToken);

            if (token != null)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                token.ReplacedByToken = replacedByToken;
            }
        }

        public async Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _dbContext.RefreshTokens
                .Where(rt => rt.UserUid == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }
        }

        public async Task<bool> IsTokenValidAsync(string token, CancellationToken cancellationToken = default)
        {
            var refreshToken = await GetByTokenAsync(token, cancellationToken);
            
            if (refreshToken == null)
                return false;

            if (refreshToken.IsRevoked)
                return false;

            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
                return false;

            return true;
        }
    }
}

