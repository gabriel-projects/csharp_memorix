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
        public IReadOnlyList<string> FilterLogins { get; }

        protected BaseQueryOptions(
            IEnumerable<Guid>? filterIds = null,
            IEnumerable<string>? filterLogins = null)
        {
            FilterIds = filterIds?.ToList() ?? [];
            FilterLogins = filterLogins?.ToList() ?? [];
        }

        public abstract class BuilderBase
        {
            protected List<Guid> _filterIds = new();
            protected List<string> _filterLogins = new();

            public TBuilder WithFilterIds<TBuilder>(IEnumerable<Guid> ids)
                where TBuilder : BuilderBase
            {
                _filterIds = ids?.ToList() ?? [];
                return (TBuilder)this;
            }

            public TBuilder WithFilterLogins<TBuilder>(IEnumerable<string> logins)
                where TBuilder : BuilderBase
            {
                _filterLogins = logins?.ToList() ?? [];
                return (TBuilder)this;
            }
        }
    }
}
