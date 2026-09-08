using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clinicAPIsSystem.Migrations
{
    /// <inheritdoc />
    public partial class lastBeforeLanch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOperations_AspNetUsers_AccountantId",
                table: "TOperations");

            migrationBuilder.AlterColumn<int>(
                name: "AccountantId",
                table: "TOperations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ReceptionistId",
                table: "TOperations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TOperations_ReceptionistId",
                table: "TOperations",
                column: "ReceptionistId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOperations_AspNetUsers_AccountantId",
                table: "TOperations",
                column: "AccountantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TOperations_AspNetUsers_ReceptionistId",
                table: "TOperations",
                column: "ReceptionistId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOperations_AspNetUsers_AccountantId",
                table: "TOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_TOperations_AspNetUsers_ReceptionistId",
                table: "TOperations");

            migrationBuilder.DropIndex(
                name: "IX_TOperations_ReceptionistId",
                table: "TOperations");

            migrationBuilder.DropColumn(
                name: "ReceptionistId",
                table: "TOperations");

            migrationBuilder.AlterColumn<int>(
                name: "AccountantId",
                table: "TOperations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TOperations_AspNetUsers_AccountantId",
                table: "TOperations",
                column: "AccountantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
