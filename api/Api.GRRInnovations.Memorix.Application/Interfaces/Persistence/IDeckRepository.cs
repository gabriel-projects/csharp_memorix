using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Persistence
{
    public interface IDeckRepository
    {
        Task<IDeck> AddDeckAsync(IDeck deckModel, IUser inUser);
        Task<IEnumerable<IDeck>> GetDecksAsync();
    }
}
