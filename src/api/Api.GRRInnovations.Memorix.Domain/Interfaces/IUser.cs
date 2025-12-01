using Api.GRRInnovations.Memorix.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Interfaces
{
    public interface IUser : IBaseModel
    {
        string Name { get; set; }
        Email Email { get; set; }
        string PasswordHash { get; set; }
        List<IDeck> Decks { get; set; }
    }
}
