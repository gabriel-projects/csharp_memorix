using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Security.Crypto
{
    public class CryptoService : ICryptoService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 10);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return hash.StartsWith("$2") && BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
