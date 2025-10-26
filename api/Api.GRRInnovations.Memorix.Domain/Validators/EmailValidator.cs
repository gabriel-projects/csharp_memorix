using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Validators
{
    public static class EmailValidator
    {
        private static readonly Regex _regex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public static bool IsValid(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && _regex.IsMatch(email);
        }
    }
}
