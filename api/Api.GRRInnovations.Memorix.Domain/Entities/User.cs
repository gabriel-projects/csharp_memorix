using Api.GRRInnovations.Memorix.Domain.Common;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Entities
{
    public class User : BaseModel, IUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public List<Deck>? DbDecks { get; set; }

        public List<IDeck>? Decks
        {
            get => DbDecks?.Cast<IDeck>()?.ToList();
            set => DbDecks = value?.Cast<Deck>()?.ToList();
        }

        public User()
        {
            DbDecks = new List<Deck>();
        }
    }
}
