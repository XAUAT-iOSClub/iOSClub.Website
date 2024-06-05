using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iOSClub.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Link = table.Column<string>(type: "varchar(256)", nullable: false),
                    Title = table.Column<string>(type: "varchar(256)", nullable: false),
                    Cover = table.Column<string>(type: "varchar(256)", nullable: false),
                    Digest = table.Column<string>(type: "varchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Link);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(33)", nullable: false),
                    DepartmentName = table.Column<string>(type: "varchar(20)", nullable: false),
                    Title = table.Column<string>(type: "varchar(20)", nullable: false),
                    Description = table.Column<string>(type: "varchar(512)", nullable: false),
                    StartTime = table.Column<string>(type: "varchar(20)", nullable: true),
                    EndTime = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(33)", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false),
                    Description = table.Column<string>(type: "varchar(512)", nullable: true),
                    Tag = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(10)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Identity = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(10)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Academy = table.Column<string>(type: "varchar(50)", nullable: false),
                    PoliticalLandscape = table.Column<string>(type: "varchar(10)", nullable: false),
                    Gender = table.Column<string>(type: "varchar(2)", nullable: false),
                    ClassName = table.Column<string>(type: "varchar(20)", nullable: false),
                    PhoneNum = table.Column<string>(type: "varchar(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(33)", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false),
                    Url = table.Column<string>(type: "varchar(64)", nullable: false),
                    IconUrl = table.Column<string>(type: "varchar(64)", nullable: false),
                    Description = table.Column<string>(type: "varchar(64)", nullable: false),
                    Tag = table.Column<string>(type: "varchar(64)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(33)", nullable: false),
                    ProjectId = table.Column<string>(type: "varchar(33)", nullable: false),
                    Title = table.Column<string>(type: "varchar(20)", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", nullable: false),
                    StartTime = table.Column<string>(type: "varchar(20)", nullable: false),
                    EndTime = table.Column<string>(type: "varchar(20)", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectModelStaffModel",
                columns: table => new
                {
                    ProjectsId = table.Column<string>(type: "varchar(33)", nullable: false),
                    StaffsUserId = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectModelStaffModel", x => new { x.ProjectsId, x.StaffsUserId });
                    table.ForeignKey(
                        name: "FK_ProjectModelStaffModel_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectModelStaffModel_Staffs_StaffsUserId",
                        column: x => x.StaffsUserId,
                        principalTable: "Staffs",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffModelTaskModel",
                columns: table => new
                {
                    TasksId = table.Column<string>(type: "varchar(33)", nullable: false),
                    UsersUserId = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffModelTaskModel", x => new { x.TasksId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_StaffModelTaskModel_Staffs_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Staffs",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffModelTaskModel_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectModelStaffModel_StaffsUserId",
                table: "ProjectModelStaffModel",
                column: "StaffsUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffModelTaskModel_UsersUserId",
                table: "StaffModelTaskModel",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "ProjectModelStaffModel");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "StaffModelTaskModel");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Tools");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
