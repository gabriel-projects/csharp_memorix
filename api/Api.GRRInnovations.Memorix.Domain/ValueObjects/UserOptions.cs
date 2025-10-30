using Api.GRRInnovations.Memorix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.ValueObjects
{
    public sealed class UserOptions : BaseQueryOptions<UserOptions, User>
    {
        public bool IncludeDecks { get; }
        public bool IncludeCards { get; }

        private UserOptions(
            IEnumerable<Guid>? filterUsers,
            IEnumerable<string>? filterLogins,
            bool includeDecks,
            bool includeCards)
            : base(filterUsers, filterLogins)
        {
            IncludeDecks = includeDecks;
            IncludeCards = includeCards;
        }

        public static Builder Create() => new Builder();

        public sealed class Builder : BuilderBase
        {
            private bool _includeDecks;
            private bool _includeCards;

            public Builder IncludeDecks(bool include = true)
            {
                _includeDecks = include;
                return this;
            }

            public Builder IncludeCards(bool include = true)
            {
                _includeCards = include;
                return this;
            }

            public UserOptions Build()
            {
                return new UserOptions(
                    filterUsers: _filterIds,
                    filterLogins: _filterLogins,
                    includeDecks: _includeDecks,
                    includeCards: _includeCards);
            }
        }
    }
}
