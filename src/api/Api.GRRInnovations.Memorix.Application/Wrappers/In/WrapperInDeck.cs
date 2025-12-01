using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.In
{
    public class WrapperInDeck<TDeck>: WrapperBase<TDeck, WrapperInDeck<TDeck>> 
        where TDeck : IDeck
    {
        [JsonPropertyName("name")]
        public string Name
        {
            get => Data.Name;
            set => Data.Name = value;
        }
    }
}
