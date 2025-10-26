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
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return model;
        }

        public async Task<bool> ExistsByEmailAsync(Email email)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email.Value.ToLowerInvariant().Trim());
        }

        public Task<IUser> GetUserAsync(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
