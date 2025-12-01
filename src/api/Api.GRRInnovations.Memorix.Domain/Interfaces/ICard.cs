using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Interfaces
{
    public interface ICard : IBaseModel
    {
        string Front { get; set; }
        string Back { get; set; }
        string? Example { get; set; }
        IDeck Deck { get; set; }
    }
}
