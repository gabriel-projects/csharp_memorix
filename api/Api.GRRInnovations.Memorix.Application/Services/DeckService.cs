using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;

        public DeckService(IDeckRepository deckRepository)
        {
            _deckRepository = deckRepository;
        }

        public Task<IDeck> AddDeckAsync(IDeck deckModel, IUser inUser)
        {
            return _deckRepository.AddDeckAsync(deckModel, inUser);
        }

        public async Task<IEnumerable<IDeck>> GetDecksAsync()
        {
            return await _deckRepository.GetDecksAsync();
        }
    }
}
