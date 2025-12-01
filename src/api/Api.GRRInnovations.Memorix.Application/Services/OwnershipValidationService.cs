using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Api.GRRInnovations.Memorix.Application.Services
{
    /// <summary>
    /// Service for validating resource ownership with centralized logic and logging
    /// </summary>
    public class OwnershipValidationService : IOwnershipValidationService
    {
        private readonly ILogger<OwnershipValidationService> _logger;

        public OwnershipValidationService(ILogger<OwnershipValidationService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void ValidateCardOwnership(ICard card, Guid userId)
        {
            if (card == null)
            {
                _logger.LogWarning("Attempted to validate ownership for null card with user {UserId}", userId);
                throw new ArgumentNullException(nameof(card), "Card cannot be null for ownership validation.");
            }

            Guid? cardOwnerId = null;

            if (card is Card cardEntity)
            {
                if (cardEntity.DbDeck?.DbUser != null)
                {
                    cardOwnerId = cardEntity.DbDeck.DbUser.Uid;
                }
                else if (cardEntity.DeckUid != Guid.Empty)
                {
                    _logger.LogWarning(
                        "Card {CardId} has DeckUid {DeckUid} but deck relationship is not loaded. Ownership validation may be incomplete.",
                        card.Uid,
                        cardEntity.DeckUid);

                    throw new InvalidOperationException(
                        "Card deck relationship must be loaded for ownership validation. Ensure repository includes the deck and user relationships.");
                }
            }

            if (!cardOwnerId.HasValue || cardOwnerId.Value != userId)
            {
                _logger.LogWarning(
                    "User {UserId} attempted to access card {CardId} owned by {CardOwnerId}",
                    userId,
                    card.Uid,
                    cardOwnerId);
                throw new UnauthorizedAccessException("You don't have permission to access this card.");
            }

            _logger.LogDebug("Card {CardId} ownership validated for user {UserId}", card.Uid, userId);
        }

        public void ValidateDeckOwnership(IDeck deck, Guid userId)
        {
            if (deck == null)
            {
                _logger.LogWarning("Attempted to validate ownership for null deck with user {UserId}", userId);
                throw new ArgumentNullException(nameof(deck), "Deck cannot be null for ownership validation.");
            }

            Guid? deckOwnerId = null;

            if (deck is Deck deckEntity)
            {
                if (deckEntity.UserUid != Guid.Empty)
                {
                    deckOwnerId = deckEntity.UserUid;
                }
                else if (deckEntity.DbUser != null)
                {
                    deckOwnerId = deckEntity.DbUser.Uid;
                }
            }

            if (!deckOwnerId.HasValue || deckOwnerId.Value != userId)
            {
                _logger.LogWarning(
                    "User {UserId} attempted to access deck {DeckId} owned by {DeckOwnerId}",
                    userId,
                    deck.Uid,
                    deckOwnerId);
                throw new UnauthorizedAccessException("You don't have permission to access this deck.");
            }

            _logger.LogDebug("Deck {DeckId} ownership validated for user {UserId}", deck.Uid, userId);
        }
    }
}

