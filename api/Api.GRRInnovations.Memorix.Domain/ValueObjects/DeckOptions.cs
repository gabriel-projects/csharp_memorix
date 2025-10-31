using Api.GRRInnovations.Memorix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.ValueObjects
{
    public sealed class DeckOptions : BaseQueryOptions<DeckOptions, Deck>
    {
        public IReadOnlyList<Guid> FilterUsersId { get; }
        public bool IncludeCards { get; }

        private DeckOptions(
            IEnumerable<Guid>? filterIds,
            IEnumerable<Guid>? filterUsersId,
            bool includeCards)
            : base(filterIds)
        {
            IncludeCards = includeCards;
            FilterUsersId = filterUsersId?.ToList() ?? [];
        }

        public static Builder Create() => new Builder();

        public sealed class Builder : BuilderBase
        {
            private bool _includeCards;
            private List<Guid> _filterUsersId = [];

            public Builder IncludeCards(bool include = true)
            {
                _includeCards = include;
                return this;
            }

            public Builder WithFilterUserId(IEnumerable<Guid> UsersIds)
            {
                _filterUsersId = UsersIds?.ToList() ?? [];
                return this;
            }

            public DeckOptions Build()
            {
                return new DeckOptions(
                    filterIds: _filterIds,
                    includeCards: _includeCards,
                    filterUsersId: _filterUsersId);
            }
        }
    }
}
