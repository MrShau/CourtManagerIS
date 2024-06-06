using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class CM_DB_0_1_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ActivityHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ActivityHistory");
        }
    }
}
