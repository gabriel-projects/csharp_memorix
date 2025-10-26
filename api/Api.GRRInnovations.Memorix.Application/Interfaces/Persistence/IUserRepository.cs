using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
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

        Task<List<IUser>> GetUsersAsync(UserOptions userOptions);

        Task<bool> ExistsByEmailAsync(Email email);
    }
}