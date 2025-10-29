using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.ValueObjects
{
    public record UserOptions
    {
        public IReadOnlyList<Guid> FilterUsers { get; }
        public IReadOnlyList<string> FilterLogins { get; }
        public bool IncludeDecks { get; }
        public bool IncludeCards { get; }

        private UserOptions(
           IEnumerable<Guid> filterUsers = null,
           IEnumerable<string> filterLogins = null,
           bool includeUserDecks = false,
           bool includeUserCards = false)
        {
            FilterUsers = filterUsers?.ToList() ?? [];
            FilterLogins = filterLogins?.ToList() ?? [];
            IncludeDecks = includeUserDecks;
            IncludeCards = includeUserCards;
        }

        public static Builder Create() => new Builder();

        public class Builder
        {
            private List<Guid> _filterUsers = new();
            private List<string> _filterLogins = new();
            private bool _includeUserDecks = false;
            private bool _includeUserCards = false;

            public Builder WithFilterUsers(IEnumerable<Guid> userUids)
            {
                _filterUsers = userUids?.ToList() ?? [];
                return this;
            }
            public Builder WithFilterLogins(IEnumerable<string> logins)
            {
                _filterLogins = logins?.ToList() ?? [];
                return this;
            }
            public Builder IncludeDecks(bool include = true)
            {
                _includeUserDecks = include;
                return this;
            }
            public Builder IncludeCards(bool include = true)
            {
                _includeUserCards = include;
                return this;
            }
            public UserOptions Build()
            {
                return new UserOptions(
                    filterUsers: _filterUsers,
                    filterLogins: _filterLogins,
                    includeUserDecks: _includeUserDecks,
                    includeUserCards: _includeUserCards);
            }
        }
    }
}
