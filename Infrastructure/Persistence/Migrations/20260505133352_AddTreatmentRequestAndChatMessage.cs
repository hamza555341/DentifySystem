using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTreatmentRequestAndChatMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Cases_CaseId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Students_StudentId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Students_AssignedStudentId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Cases_CaseId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Students_StudentId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentRatings_Cases_CaseId",
                table: "StudentRatings");

            migrationBuilder.DropIndex(
                name: "IX_Students_Email",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_PhoneNumber",
                table: "Students");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Student_Email",
                table: "Students");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Student_Phone",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_StudentRatings_CaseId_PatientId",
                table: "StudentRatings");

            migrationBuilder.DropIndex(
                name: "IX_StudentRatings_PatientId",
                table: "StudentRatings");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Email",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PhoneNumber",
                table: "Patients");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Patient_Email",
                table: "Patients");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Patient_Phone",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Cases_AssignedStudentId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_CaseId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "AssignedStudentId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "CaseId",
                table: "StudentRatings",
                newName: "TreatmentRequestId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Appointments",
                newName: "TreatmentRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_StudentId",
                table: "Appointments",
                newName: "IX_Appointments_TreatmentRequestId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Reports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CaseId",
                table: "Reports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TreatmentRequestId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Patients",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Disease",
                table: "Cases",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TreatmentRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreatmentRequests_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreatmentRequests_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreatmentRequestId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_TreatmentRequests_TreatmentRequestId",
                        column: x => x.TreatmentRequestId,
                        principalTable: "TreatmentRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_IdentityUserId",
                table: "Students",
                column: "IdentityUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentRatings_PatientId_TreatmentRequestId",
                table: "StudentRatings",
                columns: new[] { "PatientId", "TreatmentRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentRatings_TreatmentRequestId",
                table: "StudentRatings",
                column: "TreatmentRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TreatmentRequestId",
                table: "Reports",
                column: "TreatmentRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_IdentityUserId",
                table: "Patients",
                column: "IdentityUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_NationalId",
                table: "Patients",
                column: "NationalId",
                unique: true,
                filter: "[NationalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_TreatmentRequestId",
                table: "ChatMessages",
                column: "TreatmentRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRequests_CaseId",
                table: "TreatmentRequests",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRequests_StudentId",
                table: "TreatmentRequests",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_TreatmentRequests_TreatmentRequestId",
                table: "Appointments",
                column: "TreatmentRequestId",
                principalTable: "TreatmentRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Users_IdentityUserId",
                table: "Patients",
                column: "IdentityUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Cases_CaseId",
                table: "Reports",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Students_StudentId",
                table: "Reports",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_TreatmentRequests_TreatmentRequestId",
                table: "Reports",
                column: "TreatmentRequestId",
                principalTable: "TreatmentRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRatings_TreatmentRequests_TreatmentRequestId",
                table: "StudentRatings",
                column: "TreatmentRequestId",
                principalTable: "TreatmentRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Users_IdentityUserId",
                table: "Students",
                column: "IdentityUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_TreatmentRequests_TreatmentRequestId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Users_IdentityUserId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Cases_CaseId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Students_StudentId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_TreatmentRequests_TreatmentRequestId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentRatings_TreatmentRequests_TreatmentRequestId",
                table: "StudentRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Users_IdentityUserId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "TreatmentRequests");

            migrationBuilder.DropIndex(
                name: "IX_Students_IdentityUserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_StudentRatings_PatientId_TreatmentRequestId",
                table: "StudentRatings");

            migrationBuilder.DropIndex(
                name: "IX_StudentRatings_TreatmentRequestId",
                table: "StudentRatings");

            migrationBuilder.DropIndex(
                name: "IX_Reports_TreatmentRequestId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Patients_IdentityUserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_NationalId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "TreatmentRequestId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Disease",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "TreatmentRequestId",
                table: "StudentRatings",
                newName: "CaseId");

            migrationBuilder.RenameColumn(
                name: "TreatmentRequestId",
                table: "Appointments",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_TreatmentRequestId",
                table: "Appointments",
                newName: "IX_Appointments_StudentId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "Students",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Students",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Students",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Students",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CaseId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Patients",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Patients",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Patients",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AssignedStudentId",
                table: "Cases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_PhoneNumber",
                table: "Students",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Student_Email",
                table: "Students",
                sql: "Email LIKE '%_@__%.__%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Student_Phone",
                table: "Students",
                sql: "(PhoneNumber LIKE '010________' OR PhoneNumber LIKE '011________' OR PhoneNumber LIKE '012________')");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRatings_CaseId_PatientId",
                table: "StudentRatings",
                columns: new[] { "CaseId", "PatientId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentRatings_PatientId",
                table: "StudentRatings",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Email",
                table: "Patients",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PhoneNumber",
                table: "Patients",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Patient_Email",
                table: "Patients",
                sql: "Email LIKE '%_@__%.__%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Patient_Phone",
                table: "Patients",
                sql: "(PhoneNumber LIKE '010________' OR PhoneNumber LIKE '011________' OR PhoneNumber LIKE '012________')");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_AssignedStudentId",
                table: "Cases",
                column: "AssignedStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CaseId",
                table: "Appointments",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Cases_CaseId",
                table: "Appointments",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Students_StudentId",
                table: "Appointments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Students_AssignedStudentId",
                table: "Cases",
                column: "AssignedStudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Cases_CaseId",
                table: "Reports",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Students_StudentId",
                table: "Reports",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRatings_Cases_CaseId",
                table: "StudentRatings",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
