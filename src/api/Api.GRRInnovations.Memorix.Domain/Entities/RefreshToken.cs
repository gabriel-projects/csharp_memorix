using Api.GRRInnovations.Memorix.Domain.Common;
using Api.GRRInnovations.Memorix.Domain.Interfaces;

namespace Api.GRRInnovations.Memorix.Domain.Entities
{
    public class RefreshToken : BaseModel, IRefreshToken
    {
        public Guid UserUid { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }
        public User? DbUser { get; set; }
        public IUser? User
        {
            get => DbUser;
            set => DbUser = value as User;
        }
    }
}

