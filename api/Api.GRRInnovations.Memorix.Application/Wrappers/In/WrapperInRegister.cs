using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.In
{
    public class WrapperInRegister
    {
        [JsonPropertyName("login")]
        [MaxLength(150)]
        //[EmailAddress]
        public required string Email { get; set; }

        [JsonPropertyName("password")]
        [MaxLength(150)]
        public required string Password { get; set; }

        [JsonPropertyName("name")]
        [MaxLength(150)]
        public required string Name { get; set; }
    }
}
