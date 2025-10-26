using Api.GRRInnovations.Memorix.Domain.Exceptions;
using System.Net.Mail;
using System.Text.Json.Serialization;

namespace Api.GRRInnovations.Memorix.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; }

        [JsonConstructor]
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("E-mail é obrigatório.");

            value = value.Trim().ToLowerInvariant();

            if (!IsValid(value))
                throw new DomainException("E-mail inválido.");

            Value = value;
        }

        private static bool IsValid(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;
    }
}
