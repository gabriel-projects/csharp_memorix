using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Interfaces
{
    public interface IUser : IBaseModel
    {
        string Username { get; set; }
        string Email { get; set; }
        string PasswordHash { get; set; }

        List<IDeck> Decks { get; set; }
    }
}
