using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelLinkerDataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTripModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                schema: "Csc",
                table: "Trips");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "Csc",
                table: "Trips",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "Csc",
                table: "Trips",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(7)",
                oldMaxLength: 7);

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                schema: "Csc",
                table: "Trips",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
