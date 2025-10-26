using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<IUser> CreateUserAsync(IUser user);

        Task<IUser> GetUserAsync(IUser user);

        Task<bool> ExistsByEmailAsync(string email);
    }
}
