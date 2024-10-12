using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iOSClub.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tools");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(33)", nullable: false),
                    Description = table.Column<string>(type: "varchar(64)", nullable: false),
                    IconUrl = table.Column<string>(type: "varchar(64)", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false),
                    Tag = table.Column<string>(type: "varchar(64)", nullable: true),
                    Url = table.Column<string>(type: "varchar(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                });
        }
    }
}
