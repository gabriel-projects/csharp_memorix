using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.Out
{
    public class WrapperOutUser : WrapperBase<IUser, WrapperOutUser>
    {
        public WrapperOutUser() : base(null) { }

        public WrapperOutUser(IUser data) : base(data) { }

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

        [JsonPropertyName("email")]
        public string Email
        {
            get => Data.Email;
            set => Data.Email = new Domain.ValueObjects.Email(value);
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
