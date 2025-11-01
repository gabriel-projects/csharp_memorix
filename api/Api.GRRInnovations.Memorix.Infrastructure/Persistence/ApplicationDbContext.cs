using Api.GRRInnovations.Memorix.Application.Interfaces;
using Api.GRRInnovations.Memorix.Domain.Common;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
using Api.GRRInnovations.Memorix.Infrastructure.Persistence.ValueGenerators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        internal DbSet<Card> Cards { get; set; }
        internal DbSet<Deck> Decks { get; set; }
        internal DbSet<User> Users { get; set; }
        internal DbSet<RefreshToken> RefreshTokens { get; set; }

        private readonly IHttpContextAccessor? _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor? httpContextAccessor) 
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DefaultModelSetup<Card>(modelBuilder);
            modelBuilder.Entity<Card>().Ignore(x => x.Deck);
            modelBuilder.Entity<Card>().HasOne(x => x.DbDeck).WithMany(x => x.DbCards).HasForeignKey(x => x.DeckUid).OnDelete(DeleteBehavior.Cascade);

            DefaultModelSetup<Deck>(modelBuilder);
            modelBuilder.Entity<Deck>().Ignore(x => x.Cards);
            modelBuilder.Entity<Deck>().Ignore(x => x.User);
            modelBuilder.Entity<Deck>().HasOne(x => x.DbUser).WithMany(x => x.DbDecks).HasForeignKey(x => x.UserUid).OnDelete(DeleteBehavior.Cascade);

            DefaultModelSetup<User>(modelBuilder);
            modelBuilder.Entity<User>().Ignore(x => x.Decks);
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Email)
                .HasConversion(
                    email => email.Value,
                    value => new Email(value)
                )
                .HasMaxLength(150)
                .IsRequired();
            modelBuilder.Entity<User>().Property(x => x.PasswordHash).HasMaxLength(150).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.Name).HasMaxLength(150).IsRequired();

            DefaultModelSetup<RefreshToken>(modelBuilder);
            modelBuilder.Entity<RefreshToken>()
                .HasOne(x => x.DbUser)
                .WithMany()
                .HasForeignKey(x => x.UserUid)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RefreshToken>().Ignore(x => x.User);
            modelBuilder.Entity<RefreshToken>().Property(x => x.Token).HasMaxLength(500).IsRequired();
            modelBuilder.Entity<RefreshToken>().HasIndex(x => x.Token).IsUnique();
            modelBuilder.Entity<RefreshToken>().HasIndex(x => x.UserUid);
        }

        public override int SaveChanges()
        {
            AdjustChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result;
            try
            {
                AdjustChanges();
                result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }

            return result;
        }

        private void AdjustChanges()
        {
            var changes = ChangeTracker.Entries<BaseModel>().Where(p => p.State == EntityState.Modified || p.State == EntityState.Added);

            Guid? currentUserId = GetCurrentUserId();

            foreach (var entry in changes)
            {
                entry.Property(p => p.UpdatedAt).CurrentValue = DateTime.UtcNow;

                if (currentUserId.HasValue)
                {
                    entry.Property(p => p.UpdatedBy).CurrentValue = currentUserId.Value;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedAt).CurrentValue = DateTime.UtcNow;
                    if (currentUserId.HasValue)
                    {
                        entry.Property(p => p.UpdatedBy).CurrentValue = currentUserId.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current user ID from IUserContext via HttpContext
        /// Returns null if user is not authenticated or HttpContext is not available
        /// </summary>
        private Guid? GetCurrentUserId()
        {
            try
            {
                if (_httpContextAccessor?.HttpContext == null)
                    return null;

                var userContext = _httpContextAccessor.HttpContext.RequestServices?.GetService<IUserContext>();
                return userContext?.UserId;
            }
            catch
            {
                return null;
            }
        }

        public void Rollback()
        {
            var changedEntries = ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        public void DefaultModelSetup<T>(ModelBuilder modelBuilder) where T : BaseModel
        {
            modelBuilder.Entity<T>().Property(m => m.CreatedAt).IsRequired();
            modelBuilder.Entity<T>().Property(m => m.UpdatedAt).IsRequired();

            modelBuilder.Entity<T>().HasKey(m => m.Uid);
            modelBuilder.Entity<T>().Property((m) => m.Uid).IsRequired().HasValueGenerator<GuidV7ValueGenerator>();
        }
    }
}
