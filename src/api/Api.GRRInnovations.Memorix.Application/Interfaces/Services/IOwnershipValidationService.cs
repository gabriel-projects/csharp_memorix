using Api.GRRInnovations.Memorix.Domain.Interfaces;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Services
{
    /// <summary>
    /// Service for validating resource ownership to ensure users can only access their own resources
    /// </summary>
    public interface IOwnershipValidationService
    {
        /// <summary>
        /// Validates that a card belongs to the specified user
        /// </summary>
        /// <param name="card">The card to validate</param>
        /// <param name="userId">The user ID to check ownership against</param>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user does not own the card</exception>
        void ValidateCardOwnership(ICard card, Guid userId);

        /// <summary>
        /// Validates that a deck belongs to the specified user
        /// </summary>
        /// <param name="deck">The deck to validate</param>
        /// <param name="userId">The user ID to check ownership against</param>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user does not own the deck</exception>
        void ValidateDeckOwnership(IDeck deck, Guid userId);
    }
}

