using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelLinkerDataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteColumnFromRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomsImages",
                schema: "Hsc",
                table: "RoomsImages");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "Hsc",
                table: "RoomsImages");

            migrationBuilder.DropColumn(
                name: "MainImage",
                schema: "Hsc",
                table: "Rooms");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                schema: "Hsc",
                table: "Rooms",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomsImages",
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

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "Hsc",
                table: "RoomsImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                schema: "Hsc",
                table: "Rooms",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                schema: "Hsc",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomsImages",
                schema: "Hsc",
                table: "RoomsImages",
                columns: new[] { "RoomId", "ImageUrl" });
        }
    }
}
