using Api.GRRInnovations.Memorix.Domain.Common;
using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Entities
{
    public class Deck : BaseModel, IDeck
    {
        public string Name { get; set; }
        public IUser User { get; set; }

        public List<Card>? DbCards { get; set; }
        public List<ICard>? Cards
        {
            get => DbCards?.Cast<ICard>()?.ToList();
            set => DbCards = value?.Cast<Card>()?.ToList();
        }

        public Deck()
        {
            DbCards = new List<Card>();
        }
    }
}
