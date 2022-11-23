using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Migrations
{
    /// <inheritdoc />
    public partial class normadb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Seniors_SeniorId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Seniors_CareTakers_CareTakerId",
                table: "Seniors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seniors",
                table: "Seniors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CareTakers",
                table: "CareTakers");

            migrationBuilder.RenameTable(
                name: "Seniors",
                newName: "Senior");

            migrationBuilder.RenameTable(
                name: "CareTakers",
                newName: "CareTaker");

            migrationBuilder.RenameIndex(
                name: "IX_Seniors_CareTakerId",
                table: "Senior",
                newName: "IX_Senior_CareTakerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Senior",
                table: "Senior",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CareTaker",
                table: "CareTaker",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Senior_SeniorId",
                table: "Products",
                column: "SeniorId",
                principalTable: "Senior",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Senior_CareTaker_CareTakerId",
                table: "Senior",
                column: "CareTakerId",
                principalTable: "CareTaker",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Senior_SeniorId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Senior_CareTaker_CareTakerId",
                table: "Senior");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Senior",
                table: "Senior");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CareTaker",
                table: "CareTaker");

            migrationBuilder.RenameTable(
                name: "Senior",
                newName: "Seniors");

            migrationBuilder.RenameTable(
                name: "CareTaker",
                newName: "CareTakers");

            migrationBuilder.RenameIndex(
                name: "IX_Senior_CareTakerId",
                table: "Seniors",
                newName: "IX_Seniors_CareTakerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seniors",
                table: "Seniors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CareTakers",
                table: "CareTakers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Seniors_SeniorId",
                table: "Products",
                column: "SeniorId",
                principalTable: "Seniors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seniors_CareTakers_CareTakerId",
                table: "Seniors",
                column: "CareTakerId",
                principalTable: "CareTakers",
                principalColumn: "Id");
        }
    }
}
