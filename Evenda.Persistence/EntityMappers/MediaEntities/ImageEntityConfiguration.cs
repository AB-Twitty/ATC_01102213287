using Evenda.Domain.Entities.MediaEntities;
using Evenda.Persistence.EntityMappers.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evenda.Persistence.EntityMappers.MediaEntities
{
    public class ImageEntityConfiguration : BaseEntityConfiguration<Image>
    {
        public override void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Image_Extension", "extension IN ('jpg', 'jpeg', 'png')");
            });

            builder.Property(i => i.Name)
                .HasColumnName("name")
                .IsRequired(false);

            builder.Property(i => i.Path)
                .HasColumnName("path")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.Extension)
                .HasColumnName("extension")
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(i => i.IsThumbnail)
                .HasColumnName("is_thumbnail")
                .HasDefaultValue(true);

            builder.Property(i => i.EventId)
                .HasColumnName("event_id")
                .IsRequired();

            base.Configure(builder);
        }
    }
}
