using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelLinkerDataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCoverIMage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                schema: "Hsc",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                schema: "Csc",
                table: "Companies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CoverImage",
                schema: "Hsc",
                table: "Hotels",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "CoverImage",
                schema: "Csc",
                table: "Companies",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
