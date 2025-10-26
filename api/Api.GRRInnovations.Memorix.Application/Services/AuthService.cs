using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Application.Wrappers.In;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.Exceptions;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;

        public AuthService(IUserRepository userRepository, 
            ICryptoService cryptoService)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
        }

        public async Task<IUser> RegisterAsync(IUser user)
        {
            if (await _userRepository.ExistsByEmailAsync(user.Email))
                throw new DomainException("Email address is already registered.");

            user.PasswordHash = _cryptoService.HashPassword(user.PasswordHash);

            await _userRepository.CreateUserAsync(user);

            return user;
        }

        private Task<bool> CorrectPassword(string localPassword, string remotePassword)
        {
            if (remotePassword == null) return Task.FromResult(false);

            var passwordV1 = _cryptoService.HashPassword(remotePassword);

            if (localPassword == passwordV1) return Task.FromResult(true);
            if (!_cryptoService.VerifyPassword(passwordV1, localPassword)) return Task.FromResult(true);

            return Task.FromResult(false);
        }
    }
}
