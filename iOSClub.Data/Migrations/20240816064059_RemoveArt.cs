using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iOSClub.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveArt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Link = table.Column<string>(type: "varchar(256)", nullable: false),
                    Cover = table.Column<string>(type: "varchar(256)", nullable: false),
                    Digest = table.Column<string>(type: "varchar(256)", nullable: false),
                    Title = table.Column<string>(type: "varchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Link);
                });
        }
    }
}
