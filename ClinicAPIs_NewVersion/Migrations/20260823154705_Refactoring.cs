using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clinicAPIsSystem.Migrations
{
    /// <inheritdoc />
    public partial class Refactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TQualifications_QualificationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TSpecializations_SpecializationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TPrescriptions_AspNetUsers_PatientId",
                table: "TPrescriptions");

            migrationBuilder.DropTable(
                name: "MedicalPrescription");

            migrationBuilder.DropTable(
                name: "TOperations");

            migrationBuilder.DropTable(
                name: "TQualifications");

            migrationBuilder.DropTable(
                name: "TSpecializations");

            migrationBuilder.DropTable(
                name: "TMedicals");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_QualificationId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SpecializationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "TAppointments");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "TAppointments");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "TAppointments");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpecializationId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "TPrescriptions",
                newName: "MedicalRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_TPrescriptions_PatientId",
                table: "TPrescriptions",
                newName: "IX_TPrescriptions_MedicalRecordId");

            migrationBuilder.RenameColumn(
                name: "QualificationId",
                table: "AspNetUsers",
                newName: "MedicalRecordId");

            migrationBuilder.RenameColumn(
                name: "CleaningArea",
                table: "AspNetUsers",
                newName: "University");

            migrationBuilder.RenameColumn(
                name: "Accountant_YearsOfExperience",
                table: "AspNetUsers",
                newName: "GraduationYear");

            migrationBuilder.RenameColumn(
                name: "Accountant_LicenseNumber",
                table: "AspNetUsers",
                newName: "License");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "TPrescriptions",
                type: "DateTime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Dosage",
                table: "TPrescriptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "TPrescriptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "TPrescriptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "TPrescriptions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalName",
                table: "TPrescriptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TAppointments",
                type: "DateTime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "TAppointments",
                type: "DateTime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TFinancialReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthlyExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetProfit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TFinancialReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TMedicalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BloodType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMedicalRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TPaymentOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    AccountantId = table.Column<int>(type: "int", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPaymentOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPaymentOperations_AspNetUsers_AccountantId",
                        column: x => x.AccountantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPaymentOperations_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TExaminationResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ResultValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NormalRange = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    NurseId = table.Column<int>(type: "int", nullable: false),
                    MedicalRecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TExaminationResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TExaminationResults_AspNetUsers_NurseId",
                        column: x => x.NurseId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TExaminationResults_TMedicalRecords_MedicalRecordId",
                        column: x => x.MedicalRecordId,
                        principalTable: "TMedicalRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TVitalSigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodPressureSystolic = table.Column<int>(type: "int", nullable: false),
                    BloodPressureDiastolic = table.Column<int>(type: "int", nullable: false),
                    HeartRate = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OxygenSaturation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    NurseId = table.Column<int>(type: "int", nullable: false),
                    MedicalRecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVitalSigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TVitalSigns_AspNetUsers_NurseId",
                        column: x => x.NurseId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TVitalSigns_TMedicalRecords_MedicalRecordId",
                        column: x => x.MedicalRecordId,
                        principalTable: "TMedicalRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MedicalRecordId",
                table: "AspNetUsers",
                column: "MedicalRecordId",
                unique: true,
                filter: "[MedicalRecordId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhoneNumber",
                table: "AspNetUsers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TExaminationResults_MedicalRecordId",
                table: "TExaminationResults",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_TExaminationResults_NurseId",
                table: "TExaminationResults",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_TPaymentOperations_AccountantId",
                table: "TPaymentOperations",
                column: "AccountantId");

            migrationBuilder.CreateIndex(
                name: "IX_TPaymentOperations_PatientId",
                table: "TPaymentOperations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_TVitalSigns_MedicalRecordId",
                table: "TVitalSigns",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_TVitalSigns_NurseId",
                table: "TVitalSigns",
                column: "NurseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TMedicalRecords_MedicalRecordId",
                table: "AspNetUsers",
                column: "MedicalRecordId",
                principalTable: "TMedicalRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TPrescriptions_TMedicalRecords_MedicalRecordId",
                table: "TPrescriptions",
                column: "MedicalRecordId",
                principalTable: "TMedicalRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TMedicalRecords_MedicalRecordId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TPrescriptions_TMedicalRecords_MedicalRecordId",
                table: "TPrescriptions");

            migrationBuilder.DropTable(
                name: "TExaminationResults");

            migrationBuilder.DropTable(
                name: "TFinancialReports");

            migrationBuilder.DropTable(
                name: "TPaymentOperations");

            migrationBuilder.DropTable(
                name: "TVitalSigns");

            migrationBuilder.DropTable(
                name: "TMedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MedicalRecordId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "TPrescriptions");

            migrationBuilder.DropColumn(
                name: "Dosage",
                table: "TPrescriptions");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "TPrescriptions");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "TPrescriptions");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "TPrescriptions");

            migrationBuilder.DropColumn(
                name: "MedicalName",
                table: "TPrescriptions");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TAppointments");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TAppointments");

            migrationBuilder.DropColumn(
                name: "Degree",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "MedicalRecordId",
                table: "TPrescriptions",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_TPrescriptions_MedicalRecordId",
                table: "TPrescriptions",
                newName: "IX_TPrescriptions_PatientId");

            migrationBuilder.RenameColumn(
                name: "University",
                table: "AspNetUsers",
                newName: "CleaningArea");

            migrationBuilder.RenameColumn(
                name: "MedicalRecordId",
                table: "AspNetUsers",
                newName: "QualificationId");

            migrationBuilder.RenameColumn(
                name: "License",
                table: "AspNetUsers",
                newName: "Accountant_LicenseNumber");

            migrationBuilder.RenameColumn(
                name: "GraduationYear",
                table: "AspNetUsers",
                newName: "Accountant_YearsOfExperience");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDate",
                table: "TAppointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "TAppointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "TAppointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecializationId",
                table: "AspNetUsers",
                type: "int",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TMedicals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TakeTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMedicals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ReceptionistId = table.Column<int>(type: "int", nullable: false),
                    AccountantId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TOperations_AspNetUsers_AccountantId",
                        column: x => x.AccountantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TOperations_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TOperations_AspNetUsers_ReceptionistId",
                        column: x => x.ReceptionistId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TOperations_TAppointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "TAppointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TQualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Degree = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    University = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TQualifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TSpecializations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSpecializations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalPrescription",
                columns: table => new
                {
                    MedicalsId = table.Column<int>(type: "int", nullable: false),
                    PrescriptionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalPrescription", x => new { x.MedicalsId, x.PrescriptionsId });
                    table.ForeignKey(
                        name: "FK_MedicalPrescription_TMedicals_MedicalsId",
                        column: x => x.MedicalsId,
                        principalTable: "TMedicals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalPrescription_TPrescriptions_PrescriptionsId",
                        column: x => x.PrescriptionsId,
                        principalTable: "TPrescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_QualificationId",
                table: "AspNetUsers",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SpecializationId",
                table: "AspNetUsers",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalPrescription_PrescriptionsId",
                table: "MedicalPrescription",
                column: "PrescriptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_TOperations_AccountantId",
                table: "TOperations",
                column: "AccountantId");

            migrationBuilder.CreateIndex(
                name: "IX_TOperations_AppointmentId",
                table: "TOperations",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TOperations_PatientId",
                table: "TOperations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_TOperations_ReceptionistId",
                table: "TOperations",
                column: "ReceptionistId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TQualifications_QualificationId",
                table: "AspNetUsers",
                column: "QualificationId",
                principalTable: "TQualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TSpecializations_SpecializationId",
                table: "AspNetUsers",
                column: "SpecializationId",
                principalTable: "TSpecializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TPrescriptions_AspNetUsers_PatientId",
                table: "TPrescriptions",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
