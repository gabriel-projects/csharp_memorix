using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.Out
{
    public class WrapperOutDeck : WrapperBase<IDeck, WrapperOutDeck>
    {
        public WrapperOutDeck() : base(null) { }

        public WrapperOutDeck(IDeck data) : base(data) { }

        [JsonPropertyName("uid")]
        public Guid Uid
        {
            get => Data.Uid;
            set => Data.Uid = value;
        }

        [JsonPropertyName("name")]
        public string Name
        {
            get => Data.Name;
            set => Data.Name = value;
        }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt
        {
            get => Data.CreatedAt;
            set => Data.CreatedAt = value;
        }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt
        {
            get => Data.UpdatedAt;
            set => Data.UpdatedAt = value;
        }
    }
}
