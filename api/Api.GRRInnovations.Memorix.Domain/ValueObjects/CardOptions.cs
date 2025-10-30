using Api.GRRInnovations.Memorix.Domain.Entities;

namespace Api.GRRInnovations.Memorix.Domain.ValueObjects
{
    public sealed class CardOptions : BaseQueryOptions<CardOptions, Card>
    {
        public IReadOnlyList<Guid> FilterUsersId { get; }

        public Guid FilterDeckId { get; init; }

        private CardOptions(
            IEnumerable<Guid>? filterIds,
            IEnumerable<string>? filterLogins,
            IEnumerable<Guid>? filterUsersId,
            Guid filterDeckId)
            : base(filterIds, filterLogins)
        {
            FilterUsersId = filterUsersId?.ToList() ?? [];
            FilterDeckId = filterDeckId;
        }

        public static Builder Create() => new Builder();

        public sealed class Builder : BuilderBase
        {
            private List<Guid> _filterUsersId = [];

            private Guid _filterDeckId;

            public Builder WithFilterDeckId(Guid deckId)
            {
                _filterDeckId = deckId;
                return this;
            }

            public Builder WithFilterUserId(IEnumerable<Guid> UsersIds)
            {
                _filterUsersId = UsersIds?.ToList() ?? [];
                return this;
            }

            public CardOptions Build()
            {
                return new CardOptions(
                    filterIds: _filterIds,
                    filterLogins: _filterLogins,
                    filterUsersId: _filterUsersId,
                    filterDeckId: _filterDeckId);
            }
        }
    }
}
