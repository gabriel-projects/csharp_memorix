using System.Text.Json.Serialization;

namespace Api.GRRInnovations.Memorix.Application.Wrappers.In
{
    public class WrapperInRegister
    {
        [JsonPropertyName("login")]
        public required string Login { get; set; }

        [JsonPropertyName("password")]
        public required string Password { get; set; }
    }
}
