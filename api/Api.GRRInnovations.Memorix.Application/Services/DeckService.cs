using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
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
        private readonly IUnitOfWork _unitOfWork;

        public DeckService(IDeckRepository deckRepository, IUnitOfWork unitOfWork)
        {
            _deckRepository = deckRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IDeck> AddDeckAsync(IDeck deckModel, IUser inUser)
        {
            var deck = await _deckRepository.AddDeckAsync(deckModel, inUser);
            await _unitOfWork.SaveChangesAsync();
            return deck;
        }

        public async Task<IEnumerable<IDeck>> GetDecksAsync(DeckOptions options)
        {
            return await _deckRepository.GetDecksAsync(options);
        }
    }
}
