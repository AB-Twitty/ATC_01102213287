using Evenda.Domain.Entities.TicketEntities;
using Evenda.Persistence.EntityMappers.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evenda.Persistence.EntityMappers.TicketEntities
{
    public class TicketEntityConfiguration : BaseEntityConfiguration<Ticket>
    {
        public override void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("tickets");

            builder.Property(t => t.EventId).HasColumnName("event_id").IsRequired();

            builder.HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .HasConstraintName("fk_tickets_event")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.UserId).HasColumnName("user_id").IsRequired();

            builder.HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .HasConstraintName("fk_tickets_user")
                .OnDelete(DeleteBehavior.NoAction);

            base.Configure(builder);
        }
    }
}
