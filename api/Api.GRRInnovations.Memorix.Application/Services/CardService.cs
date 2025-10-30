using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;

namespace Api.GRRInnovations.Memorix.Application.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public async Task<ICard> AddCardAsync(ICard cardModel, IDeck inDeck)
        {
            return await _cardRepository.AddCardAsync(cardModel, inDeck);
        }

        public async Task<ICard> GetCardAsync(Guid uid, CardOptions options)
        {
            return await _cardRepository.GetCardAsync(uid, options);
        }

        public async Task<IEnumerable<ICard>> GetCardsAsync(CardOptions options)
        {
            return await _cardRepository.GetCardsAsync(options);
        }
    }
}
