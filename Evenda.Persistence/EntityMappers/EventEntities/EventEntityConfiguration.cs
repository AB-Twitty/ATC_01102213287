using Evenda.Domain.Entities.EventEntities;
using Evenda.Persistence.EntityMappers.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evenda.Persistence.EntityMappers.EventEntities
{
    public class EventEntityConfiguration : BaseEntityConfiguration<Event>
    {
        public override void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("events");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .IsRequired()
                .HasColumnType("ntext");

            builder.Property(e => e.Price)
                .HasColumnName("price")
                .IsRequired();

            builder.Property(e => e.Venue)
                .HasColumnName("venue")
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.Category)
                .HasColumnName("category")
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(e => e.Country)
                .HasColumnName("country")
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(e => e.City)
                .HasColumnName("city")
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(e => e.DateTime)
                .HasColumnName("date_time")
                .IsRequired();

            builder
                .HasMany(e => e.Images)
                .WithOne(i => i.Event)
                .HasForeignKey(e => e.EventId);

            builder.HasIndex(e => e.Category).IsUnique();

            base.Configure(builder);
        }
    }
}
