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
    public class DeckRepository : IDeckRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DeckRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IDeck> AddDeckAsync(IDeck deckModel, IUser inUser)
        {
            if (deckModel is not Deck deckM) return null;
            if (inUser is not User userM) return null;

            deckM.UserUid = userM.Uid;

            await _dbContext.Decks.AddAsync(deckM).ConfigureAwait(false);

            return deckM;
        }

        public async Task<List<IDeck>> GetDecksAsync(DeckOptions options)
        {
            return await Query(options).ToListAsync<IDeck>();
        }

        private IQueryable<Deck> Query(DeckOptions options)
        {
            var query = _dbContext.Decks.AsQueryable();

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
