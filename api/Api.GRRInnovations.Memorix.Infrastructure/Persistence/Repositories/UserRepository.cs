using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IUser> CreateUserAsync(IUser user)
        {
            if (user is not User model) return null;

            await _dbContext.Users.AddAsync(model).ConfigureAwait(false);

            return model;
        }

        public async Task<bool> ExistsByEmailAsync(Email email)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email.Value.ToLowerInvariant().Trim());
        }

        public async Task<IUser> GetUserAsync(Guid uid, UserOptions? userOptions = null)
        {
            return await Query(userOptions)
                   .AsNoTracking()
                   .FirstOrDefaultAsync(u => u.Uid == uid);
        }

        public async Task<List<IUser>> GetUsersAsync(UserOptions userOptions)
        {
            return await Query(userOptions).ToListAsync<IUser>();
        }

        private IQueryable<User> Query(UserOptions options)
        {
            var query = _dbContext.Users.AsQueryable();

            if (options == null)
                return query;

            if (options.FilterLogins.Any())
                query = query.Where(p => options.FilterLogins.Contains(p.Email));

            if (options.FilterIds.Any())
                query = query.Where(p => options.FilterIds.Contains(p.Uid));

            if (options.IncludeDecks)
                query = query.Include(p => p.DbDecks);

            if (options.IncludeCards)
                query = query.Include(p => p.DbDecks).ThenInclude(p => p.DbCards);

            return query;
        }
    }
}
