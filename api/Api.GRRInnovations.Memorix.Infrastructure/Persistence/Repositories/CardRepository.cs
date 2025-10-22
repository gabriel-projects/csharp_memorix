using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Persistence.Repositories
{
    public class CardRepository : ICardRepository
    {
        public Task<Card> CreateAsync(Card card)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid uid)
        {
            throw new NotImplementedException();
        }

        public Task<List<Card>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Card?> GetByUidAsync(Guid uid)
        {
            throw new NotImplementedException();
        }

        public Task<Card> UpdateAsync(Card card)
        {
            throw new NotImplementedException();
        }
    }
}
