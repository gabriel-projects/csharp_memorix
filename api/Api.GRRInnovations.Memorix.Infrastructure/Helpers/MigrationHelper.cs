using Api.GRRInnovations.Memorix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.GRRInnovations.Memorix.Infrastructure.Helpers
{
    public static class MigrationHelper
    {
        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            Console.WriteLine("Aplicando migração");

            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();

            await dbContextSvc.Database.MigrateAsync();
        }
    }
}
