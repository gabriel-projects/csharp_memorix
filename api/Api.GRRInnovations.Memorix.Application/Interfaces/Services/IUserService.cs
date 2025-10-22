using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IUser> CreateUserAsync(IUser userModel);
    }
}
