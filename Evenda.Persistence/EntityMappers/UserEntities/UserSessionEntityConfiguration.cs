using Evenda.Domain.Entities.UserEntities;
using Evenda.Persistence.EntityMappers.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evenda.Persistence.EntityMappers.UserEntities
{
    public class UserSessionEntityConfiguration : BaseEntityConfiguration<UserSession>
    {
        public override void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable("user_sessions");

            builder.Property(s => s.Token).HasColumnName("token")
                .IsRequired().HasMaxLength(200);

            builder.HasIndex(s => s.Token).IsUnique(true);

            builder.Property(s => s.ExpireAt).HasColumnName("expire_at")
                .IsRequired();

            builder.Property(s => s.UserId).HasColumnName("user_id");
            builder.HasOne(s => s.User)
                .WithMany(u => u.UserSessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
