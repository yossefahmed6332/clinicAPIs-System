using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clinicAPIsSystem.Migrations
{
    /// <inheritdoc />
    public partial class LastRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "unit",
                table: "TExaminationResults",
                newName: "Unit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "TExaminationResults",
                newName: "unit");
        }
    }
}
