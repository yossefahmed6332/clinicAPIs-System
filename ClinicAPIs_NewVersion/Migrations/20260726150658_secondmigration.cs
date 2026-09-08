using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clinicAPIsSystem.Migrations
{
    /// <inheritdoc />
    public partial class Secondmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TAppointments_AspNetUsers_NurseId",
                table: "TAppointments");

            migrationBuilder.AlterColumn<int>(
                name: "NurseId",
                table: "TAppointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "TAppointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddForeignKey(
                name: "FK_TAppointments_AspNetUsers_NurseId",
                table: "TAppointments",
                column: "NurseId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TAppointments_AspNetUsers_NurseId",
                table: "TAppointments");

            migrationBuilder.AlterColumn<int>(
                name: "NurseId",
                table: "TAppointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "TAppointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TAppointments_AspNetUsers_NurseId",
                table: "TAppointments",
                column: "NurseId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
