using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;

namespace Api.GRRInnovations.Memorix.Application.Services
{
    /// <summary>
    /// Service for user operations that coordinates between repositories and other services
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Retrieves a user by their unique identifier
        /// </summary>
        public Task<IUser> GetUserByUidAsync(Guid uid, UserOptions? userOptions = null)
        {
            return _userRepository.GetUserAsync(uid, userOptions);
        }
    }
}
