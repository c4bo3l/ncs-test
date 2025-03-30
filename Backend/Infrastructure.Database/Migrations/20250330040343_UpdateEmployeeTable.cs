using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Cafe_CafeId",
                table: "Employee");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Cafe_CafeId",
                table: "Employee",
                column: "CafeId",
                principalTable: "Cafe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Cafe_CafeId",
                table: "Employee");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Cafe_CafeId",
                table: "Employee",
                column: "CafeId",
                principalTable: "Cafe",
                principalColumn: "Id");
        }
    }
}
