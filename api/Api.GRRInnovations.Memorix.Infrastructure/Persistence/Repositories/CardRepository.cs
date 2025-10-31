using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Persistence.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CardRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<ICard> AddCardAsync(ICard cardModel, IDeck inDeck)
        {
            if (cardModel is not Card cardM) return null;
            if (inDeck is not Deck deckM) return null;

            cardM.DeckUid = deckM.Uid;

            await _dbContext.Cards.AddAsync(cardM).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return cardM;
        }

        public async Task<ICard> GetCardAsync(Guid uid, CardOptions options)
        {
            return await Query(options)
                   .AsNoTracking()
                   .FirstOrDefaultAsync(u => u.Uid == uid);
        }

        public async Task<IEnumerable<ICard>> GetCardsAsync(CardOptions options)
        {
            return await Query(options).ToListAsync<ICard>();
        }

        private IQueryable<Card> Query(CardOptions options)
        {
            var query = _dbContext.Cards.AsQueryable();

            if (options.FilterIds.Any())
                query = query.Where(p => options.FilterIds.Contains(p.Uid));

            if (options.FilterDeckId != Guid.Empty)
                query = query.Where(p => p.DeckUid == options.FilterDeckId);

            if (options.FilterUserId != Guid.Empty)
                query = query.Where(p => p.DbDeck.DbUser.Uid == options.FilterUserId);

            return query;
        }
    }
}
