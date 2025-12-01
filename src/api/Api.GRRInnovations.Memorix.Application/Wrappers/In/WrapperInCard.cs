using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.In
{
    public class WrapperInCard<TCard> : WrapperBase<TCard, WrapperInCard<TCard>>
        where TCard : ICard
    {
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
    }
}
