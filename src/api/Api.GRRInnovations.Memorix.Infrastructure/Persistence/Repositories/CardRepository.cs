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
    /// <summary>
    /// Repository for Card that uses BaseRepository for basic CRUD operations
    /// and maintains specific methods with CardOptions for complex queries
    /// </summary>
    public class CardRepository : BaseRepository<Card>, ICardRepository
    {
        public CardRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<ICard> AddCardAsync(ICard cardModel, IDeck inDeck)
        {
            if (cardModel is not Card cardM)
                throw new ArgumentException("Invalid card model type", nameof(cardModel));
            if (inDeck is not Deck deckM)
                throw new ArgumentException("Invalid deck model type", nameof(inDeck));

            cardM.DeckUid = deckM.Uid;

            return await AddAsync(cardM);
        }

        public async Task<ICard> GetCardAsync(Guid uid, CardOptions options)
        {
            var query = Query(options);
            
            if (options.FilterUserId.HasValue || options.FilterDeckId.HasValue)
            {
                query = query
                    .Include(c => c.DbDeck)
                    .ThenInclude(d => d.DbUser);
            }

            return await query.AsNoTracking()
                   .FirstOrDefaultAsync(u => u.Uid == uid);
        }

        public async Task<IEnumerable<ICard>> GetCardsAsync(CardOptions options)
        {
            return await Query(options).ToListAsync<ICard>();
        }

        private IQueryable<Card> Query(CardOptions options)
        {
            var query = _dbSet.AsQueryable();

            if (options.FilterIds.Any())
                query = query.Where(p => options.FilterIds.Contains(p.Uid));

            if (options.FilterDeckId.HasValue)
                query = query.Where(p => p.DeckUid == options.FilterDeckId.Value);

            if (options.FilterUserId.HasValue)
            {
                query = query
                    .Include(c => c.DbDeck)
                    .ThenInclude(d => d.DbUser)
                    .Where(p => p.DbDeck.DbUser.Uid == options.FilterUserId.Value);
            }

            return query;
        }
    }
}
