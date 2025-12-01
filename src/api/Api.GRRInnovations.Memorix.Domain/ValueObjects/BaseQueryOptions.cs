using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.ValueObjects
{
    public abstract class BaseQueryOptions<TOptions, TEntity>
        where TOptions : BaseQueryOptions<TOptions, TEntity>
    {
        public IReadOnlyList<Guid> FilterIds { get; }

        protected BaseQueryOptions(
            IEnumerable<Guid>? filterIds = null)
        {
            FilterIds = filterIds?.ToList() ?? [];
        }

        public abstract class BuilderBase
        {
            protected List<Guid> _filterIds = new();

            public TBuilder WithFilterIds<TBuilder>(IEnumerable<Guid> ids)
                where TBuilder : BuilderBase
            {
                _filterIds = ids?.ToList() ?? [];
                return (TBuilder)this;
            }
        }
    }
}
