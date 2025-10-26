using Api.GRRInnovations.Memorix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.In
{
    public class WrapperInLogin<TUser> : WrapperBase<TUser, WrapperInLogin<TUser>>
        where TUser : IUser
    {
        [JsonPropertyName("login")]
        [MaxLength(150)]
        [EmailAddress]
        public required string Email
        {
            get => Data.Email;
            set => Data.Email = new Domain.ValueObjects.Email(value);
        }

        [JsonPropertyName("password")]
        [MaxLength(150)]
        [MinLength(8)]
        public required string Password
        {
            get => Data.PasswordHash;
            set => Data.PasswordHash = value;
        }
    }
}
