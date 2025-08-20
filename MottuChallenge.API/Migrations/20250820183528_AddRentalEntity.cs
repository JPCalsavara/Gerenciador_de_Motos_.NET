using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuChallenge.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentalEntities",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "text", nullable: false),
                    DeliveryPersonIdentifier = table.Column<string>(type: "character varying(50)", nullable: false),
                    MotorcycleIdentifier = table.Column<string>(type: "character varying(50)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PredictedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlanDays = table.Column<int>(type: "integer", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalEntities", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_RentalEntities_DeliveryPeopleEntities_DeliveryPersonIdentif~",
                        column: x => x.DeliveryPersonIdentifier,
                        principalTable: "DeliveryPeopleEntities",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalEntities_MotorcycleEntities_MotorcycleIdentifier",
                        column: x => x.MotorcycleIdentifier,
                        principalTable: "MotorcycleEntities",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalEntities_DeliveryPersonIdentifier",
                table: "RentalEntities",
                column: "DeliveryPersonIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_RentalEntities_MotorcycleIdentifier",
                table: "RentalEntities",
                column: "MotorcycleIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalEntities");
        }
    }
}
