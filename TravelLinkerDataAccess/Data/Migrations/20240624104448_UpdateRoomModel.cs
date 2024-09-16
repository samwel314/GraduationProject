using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelLinkerDataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedFrom",
                schema: "Hsc",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ReservedTo",
                schema: "Hsc",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                schema: "Hsc",
                table: "Rooms",
                newName: "IsHide");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsHide",
                schema: "Hsc",
                table: "Rooms",
                newName: "IsAvailable");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservedFrom",
                schema: "Hsc",
                table: "Rooms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservedTo",
                schema: "Hsc",
                table: "Rooms",
                type: "datetime2",
                nullable: true);
        }
    }
}
