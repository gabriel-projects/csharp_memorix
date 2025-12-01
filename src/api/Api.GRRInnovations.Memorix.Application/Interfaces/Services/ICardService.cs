using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Services
{
    public interface ICardService
    {
        Task<ICard> AddCardAsync(ICard cardModel, IDeck inDeck);
        Task<ICard> GetCardForUserAsync(Guid cardId, CardOptions options);
        Task<IEnumerable<ICard>> GetCardsAsync(CardOptions options);
    }
}
