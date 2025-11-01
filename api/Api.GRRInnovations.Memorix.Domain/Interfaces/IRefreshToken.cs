using Api.GRRInnovations.Memorix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Interfaces
{
    public interface IRefreshToken : IBaseModel
    {
        Guid UserUid { get; set; }
        string Token { get; set; } 
        DateTime ExpiresAt { get; set; }
        bool IsRevoked { get; set; }
        DateTime? RevokedAt { get; set; }
        string? ReplacedByToken { get; set; }
        IUser User { get; set; }
    }
}
