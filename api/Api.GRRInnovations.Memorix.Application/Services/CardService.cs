using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;

namespace Api.GRRInnovations.Memorix.Application.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CardService(ICardRepository cardRepository, IUnitOfWork unitOfWork)
        {
            _cardRepository = cardRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICard> AddCardAsync(ICard cardModel, IDeck inDeck)
        {
            var card = await _cardRepository.AddCardAsync(cardModel, inDeck);
            await _unitOfWork.SaveChangesAsync();
            return card;
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
