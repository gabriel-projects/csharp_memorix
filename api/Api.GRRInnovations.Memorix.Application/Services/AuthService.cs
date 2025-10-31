using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Exceptions;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;

namespace Api.GRRInnovations.Memorix.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUserRepository userRepository, 
            ICryptoService cryptoService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IUser> RegisterAsync(IUser user)
        {
            if (await _userRepository.ExistsByEmailAsync(user.Email))
                throw new DomainException("Email address is already registered.");

            user.PasswordHash = _cryptoService.HashPassword(user.PasswordHash);

            await _userRepository.CreateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        public async Task<IUser> ValidateAsync(string login, string password)
        {
            var options = UserOptions.Create()
                .WithFilterLogins(new List<string> { login })
                .Build();

            var users = await _userRepository.GetUsersAsync(options);

            var remoteUser = users.FirstOrDefault();
            if (remoteUser != null && !CorrectPassword(password, remoteUser.PasswordHash))
            {
                remoteUser = null;
            }

            //todo: validate user active or blocked

            return remoteUser;
        }

        private bool CorrectPassword(string inputPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(inputPassword))
                return false;

            return _cryptoService.VerifyPassword(inputPassword, storedHash);
        }
    }
}
