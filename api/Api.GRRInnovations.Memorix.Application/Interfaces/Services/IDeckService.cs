using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Services
{
    public interface IDeckService
    {
        Task<IDeck> AddDeckAsync(IDeck deckModel, IUser inUser);
        Task<IEnumerable<IDeck>> GetDecksAsync(DeckOptions options);
    }
}
