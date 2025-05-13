using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evenda.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_TicketsQty_Prop_Events_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_events_category",
                table: "events");

            migrationBuilder.AddColumn<int>(
                name: "tickets_quantity",
                table: "events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_events_category",
                table: "events",
                column: "category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_events_category",
                table: "events");

            migrationBuilder.DropColumn(
                name: "tickets_quantity",
                table: "events");

            migrationBuilder.CreateIndex(
                name: "IX_events_category",
                table: "events",
                column: "category",
                unique: true);
        }
    }
}
