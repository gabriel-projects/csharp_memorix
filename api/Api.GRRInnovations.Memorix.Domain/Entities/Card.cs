using Api.GRRInnovations.Memorix.Domain.Common;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Entities
{
    public class Card : BaseModel, ICard
    {
        public string Front { get; set; }
        public string Back { get; set; }
        public string? Example { get; set; }

        public Guid DeckUid { get; set; }
        public Deck? DbDeck { get; set; }
        public IDeck? Deck
        {
            get => DbDeck;
            set => DbDeck = value as Deck;
        }
    }
}
