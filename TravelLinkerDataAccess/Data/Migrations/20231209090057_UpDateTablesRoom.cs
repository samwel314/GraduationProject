using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelLinkerDataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpDateTablesRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoosmFeatures",
                schema: "Hsc",
                table: "RoosmFeatures");

            migrationBuilder.DropIndex(
                name: "IX_RoosmFeatures_RoomId",
                schema: "Hsc",
                table: "RoosmFeatures");

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
                table: "RoosmFeatures");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Hsc",
                table: "RoomsImages");

            migrationBuilder.AlterColumn<string>(
                name: "FeatureName",
                schema: "Hsc",
                table: "RoosmFeatures",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                schema: "Hsc",
                table: "RoomsImages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoosmFeatures",
                schema: "Hsc",
                table: "RoosmFeatures",
                columns: new[] { "RoomId", "FeatureName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomsImages",
                schema: "Hsc",
                table: "RoomsImages",
                columns: new[] { "RoomId", "ImageUrl" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoosmFeatures",
                schema: "Hsc",
                table: "RoosmFeatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomsImages",
                schema: "Hsc",
                table: "RoomsImages");

            migrationBuilder.AlterColumn<string>(
                name: "FeatureName",
                schema: "Hsc",
                table: "RoosmFeatures",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Hsc",
                table: "RoosmFeatures",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                schema: "Hsc",
                table: "RoomsImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Hsc",
                table: "RoomsImages",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoosmFeatures",
                schema: "Hsc",
                table: "RoosmFeatures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomsImages",
                schema: "Hsc",
                table: "RoomsImages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoosmFeatures_RoomId",
                schema: "Hsc",
                table: "RoosmFeatures",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsImages_RoomId",
                schema: "Hsc",
                table: "RoomsImages",
                column: "RoomId");
        }
    }
}
