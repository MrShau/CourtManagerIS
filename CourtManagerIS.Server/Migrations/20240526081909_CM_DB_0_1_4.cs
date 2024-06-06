using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class CM_DB_0_1_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departments_SupervisorId",
                table: "Departments");

            migrationBuilder.AlterColumn<int>(
                name: "SupervisorId",
                table: "Departments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SupervisorId",
                table: "Departments",
                column: "SupervisorId",
                unique: true,
                filter: "[SupervisorId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departments_SupervisorId",
                table: "Departments");

            migrationBuilder.AlterColumn<int>(
                name: "SupervisorId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SupervisorId",
                table: "Departments",
                column: "SupervisorId",
                unique: true);
        }
    }
}
