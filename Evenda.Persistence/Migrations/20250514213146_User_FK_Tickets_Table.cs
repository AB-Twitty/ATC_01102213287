using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evenda.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class User_FK_Tickets_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tickets_event",
                table: "tickets");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "tickets",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_tickets_user_id",
                table: "tickets",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_event",
                table: "tickets",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_user",
                table: "tickets",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tickets_event",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "fk_tickets_user",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "IX_tickets_user_id",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "tickets");

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_event",
                table: "tickets",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
