using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Domain.Common;
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
    /// <summary>
    /// Repository for Deck that uses BaseRepository for basic CRUD operations
    /// and maintains specific methods with DeckOptions for complex queries
    /// </summary>
    public class DeckRepository : BaseRepository<Deck>, IDeckRepository
    {
        public DeckRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IDeck> AddDeckAsync(IDeck deckModel, IUser inUser)
        {
            if (deckModel is not Deck deckM)
                throw new ArgumentException("Invalid deck model type", nameof(deckModel));
            if (inUser is not User userM)
                throw new ArgumentException("Invalid user model type", nameof(inUser));

            deckM.UserUid = userM.Uid;

            return await AddAsync(deckM);
        }

        public async Task<List<IDeck>> GetDecksAsync(DeckOptions options)
        {
            return await Query(options).ToListAsync<IDeck>();
        }

        private IQueryable<Deck> Query(DeckOptions options)
        {
            var query = _dbSet.AsQueryable();

            if (options.FilterIds.Any())
                query = query.Where(p => options.FilterIds.Contains(p.Uid));

            if (options.FilterUsersId.Any())
                query = query.Where(p => options.FilterUsersId.Contains(p.UserUid));

            if (options.IncludeCards)
                query = query.Include(p => p.DbCards);

            return query;
        }
    }
}
