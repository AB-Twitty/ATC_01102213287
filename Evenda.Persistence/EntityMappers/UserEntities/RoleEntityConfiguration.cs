using Evenda.Domain.Entities.UserEntities;
using Evenda.Persistence.EntityMappers.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evenda.Persistence.EntityMappers.UserEntities
{
    public class RoleEntityConfiguration : BaseEntityConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            builder.Property(r => r.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.SystemName)
                .HasColumnName("system_name")
                .HasComputedColumnSql("CONCAT('role_', UPPER(name))")
                .IsRequired()
                .HasMaxLength(60);

            builder.HasData(
                new Role
                {
                    Id = new Guid("3BAB131C-EC00-4FD4-A171-8B2F15CD582A"),
                    Name = "Admin",
                    SystemName = "role_ADMIN",
                    IsDeleted = false
                },
                new Role
                {
                    Id = new Guid("D6105333-1094-48D9-B85C-8F643F780335"),
                    Name = "Customer",
                    SystemName = "role_CUSTOMER",
                    IsDeleted = false
                }
                );

            base.Configure(builder);
        }
    }
}
