using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iOSClub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identity",
                table: "Articles",
                type: "varchar(20)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identity",
                table: "Articles");
        }
    }
}
