using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evenda.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Modify_Columns_Images_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Image_Extension",
                table: "images");

            migrationBuilder.DropColumn(
                name: "content_stream",
                table: "images");

            migrationBuilder.DropColumn(
                name: "extension",
                table: "images");

            migrationBuilder.AddColumn<string>(
                name: "content_type",
                table: "images",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "image_stream",
                table: "images",
                type: "varbinary(MAX)",
                nullable: false);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Image_Content_Type",
                table: "images",
                sql: "content_type IN ('image/jpg', 'image/jpeg', 'image/png')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Image_Content_Type",
                table: "images");

            migrationBuilder.DropColumn(
                name: "content_type",
                table: "images");

            migrationBuilder.DropColumn(
                name: "image_stream",
                table: "images");

            migrationBuilder.AddColumn<byte[]>(
                name: "content_stream",
                table: "images",
                type: "varbinary",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "extension",
                table: "images",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Image_Extension",
                table: "images",
                sql: "extension IN ('jpg', 'jpeg', 'png')");
        }
    }
}
