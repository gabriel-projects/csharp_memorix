using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.Out
{
    public class WrapperOutCard : WrapperBase<ICard, WrapperOutCard>
    {
        public WrapperOutCard() : base(null) { }

        public WrapperOutCard(ICard data) : base(data) { }

        [JsonPropertyName("uid")]
        public Guid Uid
        {
            get => Data.Uid;
            set => Data.Uid = value;
        }

        [JsonPropertyName("front")]
        public string Front
        {
            get => Data.Front;
            set => Data.Front = value;
        }

        [JsonPropertyName("back")]
        public string Back
        {
            get => Data.Back;
            set => Data.Back = value;
        }

        [JsonPropertyName("example")]
        public string Example
        {
            get => Data.Example;
            set => Data.Example = value;
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
