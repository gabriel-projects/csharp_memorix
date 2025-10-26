using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.In
{
    public class WrapperInRegister<TUser> : WrapperBase<TUser, WrapperInRegister<TUser>>
        where TUser : IUser
    {
        [JsonPropertyName("login")]
        [MaxLength(150)]
        [EmailAddress]
        public required string Email
        {
            get => Data.Email;
            set => Data.Email = value;
        }

        [JsonPropertyName("password")]
        [MaxLength(150)]
        public required string Password
        {
            get => Data.PasswordHash;
            set => Data.PasswordHash = value;
        }

        [JsonPropertyName("name")]
        [MaxLength(150)]
        public required string Name
        {
            get => Data.Name;
            set => Data.Name = value;
        }
    }
}
