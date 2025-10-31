using Api.GRRInnovations.Memorix.Domain.Entities;

namespace Api.GRRInnovations.Memorix.Domain.ValueObjects
{
    public sealed class UserOptions : BaseQueryOptions<UserOptions, User>
    {
        public bool IncludeDecks { get; }
        public bool IncludeCards { get; }
        public IReadOnlyList<string> FilterLogins { get; }

        private UserOptions(
            IEnumerable<Guid>? filterIds,
            IEnumerable<string>? filterLogins,
            bool includeDecks,
            bool includeCards)
            : base(filterIds)
        {
            IncludeDecks = includeDecks;
            IncludeCards = includeCards;
            FilterLogins = filterLogins?.ToList() ?? [];
        }

        public static Builder Create() => new Builder();

        public sealed class Builder : BuilderBase
        {
            private bool _includeDecks;
            private bool _includeCards;
            private List<string>? _filterLogins;

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

            public Builder WithFilterLogins(IEnumerable<string> logins)
            {
                _filterLogins = logins?.ToList() ?? [];
                return this;
            }

            public UserOptions Build()
            {
                return new UserOptions(
                    filterIds: _filterIds,
                    filterLogins: _filterLogins,
                    includeDecks: _includeDecks,
                    includeCards: _includeCards);
            }
        }
    }
}
