using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.Out
{
    public class WrapperOutUserFull : WrapperOutUser
    {
        public WrapperOutUserFull() : base(null) { }

        public WrapperOutUserFull(IUser data) : base(data) { }


        [JsonPropertyName("Decks")]
        public List<WrapperOutDeck> Decks { get; set; }

        public override async Task Populate(IUser data)
        {
            await base.Populate(data).ConfigureAwait(false);

            if (data.Decks != null)
            {
                Decks = await WrapperOutDeck.From(data.Decks).ConfigureAwait(false);
            }
        }

        public static async Task<WrapperOutUserFull> From<TUser>(TUser data) where TUser : IUser
        {
            var result = new WrapperOutUserFull(data);
            await result.Populate(data);

            return result;
        }

        public static async Task<List<WrapperOutUserFull>> From<TUser>(List<TUser> data) where TUser : IUser
        {
            var result = new List<WrapperOutUserFull>();

            foreach (var prop in data)
            {
                var wd = await From(prop).ConfigureAwait(false);
                result.Add(wd);
            }

            return result;
        }
    }
}
