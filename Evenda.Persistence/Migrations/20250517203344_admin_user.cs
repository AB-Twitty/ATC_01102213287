using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evenda.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class admin_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "first_name", "last_modified", "last_name", "password_hash" },
                values: new object[] { new Guid("3bab131c-ec00-4fd4-a171-8b2f15cd502a"), "Admin@evenda.com", "Admin", null, "Evenda", "kvf5X6s0JCzriZN85+0wig==;qg4dWyhAc0bidwvyQurTOfoOJ5dNVfU5n1/IX0SBLeU=" });

            migrationBuilder.InsertData(
                table: "users_roles",
                columns: new[] { "role_id", "user_id" },
                values: new object[] { new Guid("3bab131c-ec00-4fd4-a171-8b2f15cd582a"), new Guid("3bab131c-ec00-4fd4-a171-8b2f15cd502a") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users_roles",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { new Guid("3bab131c-ec00-4fd4-a171-8b2f15cd582a"), new Guid("3bab131c-ec00-4fd4-a171-8b2f15cd502a") });

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("3bab131c-ec00-4fd4-a171-8b2f15cd502a"));
        }
    }
}
