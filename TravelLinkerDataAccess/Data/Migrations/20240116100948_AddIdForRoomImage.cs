using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelLinkerDataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdForRoomImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomsImages",
                schema: "Hsc",
                table: "RoomsImages");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Hsc",
                table: "RoomsImages",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomsImages",
                schema: "Hsc",
                table: "RoomsImages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsImages_RoomId",
                schema: "Hsc",
                table: "RoomsImages",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomsImages",
                schema: "Hsc",
                table: "RoomsImages");

            migrationBuilder.DropIndex(
                name: "IX_RoomsImages_RoomId",
                schema: "Hsc",
                table: "RoomsImages");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Hsc",
                table: "RoomsImages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomsImages",
                schema: "Hsc",
                table: "RoomsImages",
                column: "RoomId");
        }
    }
}
