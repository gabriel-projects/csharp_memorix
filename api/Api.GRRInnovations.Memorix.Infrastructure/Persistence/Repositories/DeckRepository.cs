using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Domain.Common;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
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
        private readonly ApplicationDbContext Context;

        public DeckRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IDeck> AddDeckAsync(IDeck deckModel, IUser inUser)
        {
            if (deckModel is not Deck deckM) return null;
            if (inUser is not User userM) return null;

            deckM.User = userM;

            await Context.Decks.AddAsync(deckM).ConfigureAwait(false);
            await Context.SaveChangesAsync().ConfigureAwait(false);

            return deckM;
        }

        public async Task<IEnumerable<IDeck>> GetDecksAsync()
        {
            return await Context.Decks.ToListAsync();
        }
    }
}
