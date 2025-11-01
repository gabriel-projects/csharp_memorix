using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
using System;

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

        //todo: teste get card for a different user
        public async Task<ICard> GetCardForUserAsync(Guid cardId, CardOptions options)
        {
            var card = await _cardRepository.GetCardAsync(cardId, options);
            
            if (card == null)
                return null;

            var userId = options.FilterUserId;

            if (card is Domain.Entities.Card cardEntity)
            {
                if (cardEntity.DbDeck.DbUser.Uid != userId)
                {
                    throw new UnauthorizedAccessException("You don't have permission to access this card.");
                }
            }

            return card;
        }

        public async Task<IEnumerable<ICard>> GetCardsAsync(CardOptions options)
        {
            return await _cardRepository.GetCardsAsync(options);
        }
    }
}
