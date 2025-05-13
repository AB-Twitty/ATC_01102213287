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
            builder.ToTable("images", t =>
            {
                t.HasCheckConstraint("CK_Image_Content_Type", "content_type IN ('image/jpg', 'image/jpeg', 'image/png')");
            });

            builder.Property(i => i.Name)
                .HasColumnName("name")
                .IsRequired(false);

            builder.Property(i => i.ImageStream)
                .HasColumnName("image_stream")
                .IsRequired()
                .HasColumnType("varbinary(MAX)");


            builder.Property(i => i.ContentType)
                .HasColumnName("content_type")
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(i => i.IsThumbnail)
                .HasColumnName("is_thumbnail")
                .HasDefaultValue(false);

            builder.Property(i => i.EventId)
                .HasColumnName("event_id")
                .IsRequired();

            base.Configure(builder);
        }
    }
}
