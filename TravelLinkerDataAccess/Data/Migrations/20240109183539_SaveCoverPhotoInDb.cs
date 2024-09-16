using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelLinkerDataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class SaveCoverPhotoInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                schema: "Hsc",
                table: "Hotels");

            migrationBuilder.AddColumn<byte[]>(
                name: "CoverImage",
                schema: "Hsc",
                table: "Hotels",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                schema: "Hsc",
                table: "Hotels");

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                schema: "Hsc",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
