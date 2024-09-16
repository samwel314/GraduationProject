using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelLinkerDataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                schema: "Hsc",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    FloorNumber = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MainImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BedsNumber = table.Column<int>(type: "int", nullable: false),
                    ReservedFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReservedTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    PricePerNight = table.Column<double>(type: "float", nullable: false),
                    PricePerMonth = table.Column<double>(type: "float", nullable: false),
                    PricePerWeek = table.Column<double>(type: "float", nullable: false),
                    HotelId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalSchema: "Hsc",
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomsImages",
                schema: "Hsc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomsImages_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Hsc",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomsOffers",
                schema: "Hsc",
                columns: table => new
                {
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfferPrice = table.Column<double>(type: "float", nullable: false),
                    OfferDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsOffers", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_RoomsOffers_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Hsc",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoosmFeatures",
                schema: "Hsc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeatureName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoosmFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoosmFeatures_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Hsc",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId",
                schema: "Hsc",
                table: "Rooms",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsImages_RoomId",
                schema: "Hsc",
                table: "RoomsImages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoosmFeatures_RoomId",
                schema: "Hsc",
                table: "RoosmFeatures",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomsImages",
                schema: "Hsc");

            migrationBuilder.DropTable(
                name: "RoomsOffers",
                schema: "Hsc");

            migrationBuilder.DropTable(
                name: "RoosmFeatures",
                schema: "Hsc");

            migrationBuilder.DropTable(
                name: "Rooms",
                schema: "Hsc");
        }
    }
}
