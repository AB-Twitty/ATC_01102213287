using Evenda.Domain.Entities.UserEntities;
using Evenda.Persistence.EntityMappers.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserEntityConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(u => u.FirstName)
               .HasColumnName("first_name")
               .IsUnicode(true)
               .IsRequired()
               .HasMaxLength(30);

        builder.Property(u => u.LastName)
               .HasColumnName("last_name")
               .IsUnicode(true)
               .IsRequired()
               .HasMaxLength(30);

        builder.Property(u => u.Email)
               .HasColumnName("email")
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(u => u.PasswordHash)
               .HasColumnName("password_hash")
               .IsRequired();

        builder
               .HasMany(u => u.Roles)
               .WithMany(r => r.Users)
               .UsingEntity<Dictionary<string, object>>(
                    "users_roles",
                    right => right
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("role_id")
                        .HasPrincipalKey(r => r.Id)
                        .OnDelete(DeleteBehavior.Cascade),
                    left => left
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("user_id")
                        .HasPrincipalKey(u => u.Id)
                        .OnDelete(DeleteBehavior.NoAction),
                    join =>
                    {
                        join.HasKey("user_id", "role_id");
                    }
               );

        base.Configure(builder);
    }
}
