using Evenda.Domain.Base;
using Evenda.Domain.Entities.EventEntities;
using Evenda.Domain.Entities.MediaEntities;
using Evenda.Domain.Entities.TagEntities;
using Evenda.Domain.Entities.TicketEntities;
using Evenda.Domain.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Evenda.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModified = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.IsDeleted = true;
                Entry(entity).State = EntityState.Modified;
                return Entry(entity);
            }

            return base.Remove(entity);
        }

        #region DbSets

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserSession> UserSessions { get; set; }

        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<Ticket> Tickets { get; set; }

        #endregion
    }
}
