using Api.GRRInnovations.Memorix.Domain.Entities;

namespace Api.GRRInnovations.Memorix.Domain.ValueObjects
{
    public sealed class CardOptions : BaseQueryOptions<CardOptions, Card>
    {
        public Guid FilterUserId { get; }

        public Guid FilterDeckId { get; init; }

        private CardOptions(
            IEnumerable<Guid>? filterIds,
            Guid filterUserId,
            Guid filterDeckId)
            : base(filterIds)
        {
            FilterUserId = filterUserId;
            FilterDeckId = filterDeckId;
        }

        public static Builder Create() => new Builder();

        public sealed class Builder : BuilderBase
        {
            private Guid _filterUserId;

            private Guid _filterDeckId;

            public Builder WithFilterDeckId(Guid deckId)
            {
                _filterDeckId = deckId;
                return this;
            }

            public Builder WithFilterUserId(Guid userId)
            {
                _filterUserId = userId;
                return this;
            }

            public CardOptions Build()
            {
                return new CardOptions(
                    filterIds: _filterIds,
                    filterUserId: _filterUserId,
                    filterDeckId: _filterDeckId);
            }
        }
    }
}
