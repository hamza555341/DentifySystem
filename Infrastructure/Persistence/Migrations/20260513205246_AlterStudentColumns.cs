using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterStudentColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "University",
                table: "Students",
                newName: "City");

            migrationBuilder.AddColumn<string>(
                name: "UniEmail",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniEmail",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Students",
                newName: "University");

            migrationBuilder.AddColumn<int>(
                name: "AcademicYear",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
