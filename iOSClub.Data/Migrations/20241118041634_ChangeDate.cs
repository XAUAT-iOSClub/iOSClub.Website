using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iOSClub.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinTime",
                table: "Students",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "Staffs",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                table: "Projects",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_DepartmentName",
                table: "Staffs",
                column: "DepartmentName");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DepartmentName",
                table: "Projects",
                column: "DepartmentName");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Departments_DepartmentName",
                table: "Projects",
                column: "DepartmentName",
                principalTable: "Departments",
                principalColumn: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Departments_DepartmentName",
                table: "Staffs",
                column: "DepartmentName",
                principalTable: "Departments",
                principalColumn: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Departments_DepartmentName",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Departments_DepartmentName",
                table: "Staffs");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_DepartmentName",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Projects_DepartmentName",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Staffs");

            migrationBuilder.AlterColumn<string>(
                name: "JoinTime",
                table: "Students",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATE");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                table: "Projects",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);
        }
    }
}
