using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuChallenge.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryPeopleEntities",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CnhNumber = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    CnhType = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CnhImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPeopleEntities", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "MotorcycleEntities",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LicensePlate = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorcycleEntities", x => x.Identifier);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPeopleEntities_CnhNumber",
                table: "DeliveryPeopleEntities",
                column: "CnhNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPeopleEntities_Cnpj",
                table: "DeliveryPeopleEntities",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleEntities_LicensePlate",
                table: "MotorcycleEntities",
                column: "LicensePlate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryPeopleEntities");

            migrationBuilder.DropTable(
                name: "MotorcycleEntities");
        }
    }
}
