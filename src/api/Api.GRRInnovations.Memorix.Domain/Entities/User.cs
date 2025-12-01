using Api.GRRInnovations.Memorix.Domain.Common;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Entities
{
    public class User : BaseModel, IUser
    {
        public string Name { get; set; } = string.Empty;

        public Email Email { get; set; } = new("placeholder@email.com");

        public string PasswordHash { get; set; } = string.Empty;

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
