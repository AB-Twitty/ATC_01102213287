using Evenda.Domain.Entities.EventEntities;
using Evenda.Domain.Entities.TagEntities;
using Evenda.Persistence.EntityMappers.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evenda.Persistence.EntityMappers.TagEntities
{
    public class TagEntityConfiguration : BaseEntityConfiguration<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("tags");

            builder.Property(t => t.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);

            builder
               .HasMany(t => t.Events)
               .WithMany(e => e.Tags)
               .UsingEntity<Dictionary<string, object>>(
                    "events_tags",
                    right => right
                        .HasOne<Event>()
                        .WithMany()
                        .HasForeignKey("event_id")
                        .HasPrincipalKey(e => e.Id)
                        .OnDelete(DeleteBehavior.NoAction),
                    left => left
                        .HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("tag_id")
                        .HasPrincipalKey(t => t.Id)
                        .OnDelete(DeleteBehavior.NoAction),
                    join =>
                    {
                        join.HasKey("event_id", "tag_id");
                    }
               );

            base.Configure(builder);
        }
    }
}
