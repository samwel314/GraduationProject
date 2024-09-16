using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelLinkerDataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeTripTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleSchedules_Trips_TripId",
                schema: "Csc",
                table: "VehicleSchedules");

            migrationBuilder.DropIndex(
                name: "IX_VehicleSchedules_TripId",
                schema: "Csc",
                table: "VehicleSchedules");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSchedules_TripId",
                schema: "Csc",
                table: "VehicleSchedules",
                column: "TripId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleSchedules_Trips_TripId",
                schema: "Csc",
                table: "VehicleSchedules",
                column: "TripId",
                principalSchema: "Csc",
                principalTable: "Trips",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleSchedules_Trips_TripId",
                schema: "Csc",
                table: "VehicleSchedules");

            migrationBuilder.DropIndex(
                name: "IX_VehicleSchedules_TripId",
                schema: "Csc",
                table: "VehicleSchedules");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSchedules_TripId",
                schema: "Csc",
                table: "VehicleSchedules",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleSchedules_Trips_TripId",
                schema: "Csc",
                table: "VehicleSchedules",
                column: "TripId",
                principalSchema: "Csc",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
