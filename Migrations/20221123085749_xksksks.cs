using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Migrations
{
    /// <inheritdoc />
    public partial class xksksks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Senior_SeniorId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Senior");

            migrationBuilder.DropTable(
                name: "CareTaker");

            migrationBuilder.DropIndex(
                name: "IX_Products_SeniorId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SeniorId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeniorId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CareTaker",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareTaker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Senior",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CareTakerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HouseNr = table.Column<int>(type: "int", nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Senior", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Senior_CareTaker_CareTakerId",
                        column: x => x.CareTakerId,
                        principalTable: "CareTaker",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SeniorId",
                table: "Products",
                column: "SeniorId");

            migrationBuilder.CreateIndex(
                name: "IX_Senior_CareTakerId",
                table: "Senior",
                column: "CareTakerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Senior_SeniorId",
                table: "Products",
                column: "SeniorId",
                principalTable: "Senior",
                principalColumn: "Id");
        }
    }
}
