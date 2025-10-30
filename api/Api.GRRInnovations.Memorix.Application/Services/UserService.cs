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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IUser> CreateUserAsync(IUser userModel)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> GetUserByUidAsync(Guid uid, UserOptions? userOptions = null)
        {
            return _userRepository.GetUserAsync(uid, userOptions);
        }
    }
}
