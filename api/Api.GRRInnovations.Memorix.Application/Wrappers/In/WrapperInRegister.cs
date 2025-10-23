using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.In
{
    public class WrapperInRegister
    {
        [JsonPropertyName("login")]
        [MaxLength(50)]
        public required string Email { get; set; }

        [JsonPropertyName("password")]
        [MaxLength(30)]
        public required string Password { get; set; }

        [JsonPropertyName("Name")]
        [MaxLength(100)]
        public required string Name { get; set; }
    }
}
