using Api.GRRInnovations.Memorix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Persistence
{
    public interface ICardRepository 
    {
        Task<List<Card>> GetAllAsync();

        Task<Card?> GetByUidAsync(Guid uid);

        Task<Card> CreateAsync(Card card);

        Task<Card> UpdateAsync(Card card);

        Task<bool> DeleteAsync(Guid uid);
    }
}
