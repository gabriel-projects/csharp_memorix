using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Api.GRRInnovations.Memorix.Infrastructure.Persistence.ValueGenerators
{
    public class GuidV7ValueGenerator : ValueGenerator<Guid>
    {
        public override bool GeneratesTemporaryValues => false;

        public override Guid Next(EntityEntry entry)
        {
            return Guid.CreateVersion7();
        }
    }
}
