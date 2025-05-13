using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evenda.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContentStream_Bytes_Images_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_events_event_id",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "path",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "images");

            migrationBuilder.RenameIndex(
                name: "IX_Images_event_id",
                table: "images",
                newName: "IX_images_event_id");

            migrationBuilder.AlterColumn<bool>(
                name: "is_thumbnail",
                table: "images",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "content_stream",
                table: "images",
                type: "varbinary",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_images",
                table: "images",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_images_events_event_id",
                table: "images",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_images_events_event_id",
                table: "images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_images",
                table: "images");

            migrationBuilder.DropColumn(
                name: "content_stream",
                table: "images");

            migrationBuilder.RenameTable(
                name: "images",
                newName: "Images");

            migrationBuilder.RenameIndex(
                name: "IX_images_event_id",
                table: "Images",
                newName: "IX_Images_event_id");

            migrationBuilder.AlterColumn<bool>(
                name: "is_thumbnail",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "path",
                table: "Images",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_events_event_id",
                table: "Images",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
