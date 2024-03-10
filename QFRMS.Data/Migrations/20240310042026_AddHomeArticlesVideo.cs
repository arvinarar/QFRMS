using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QFRMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeArticlesVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomePageArticlesVideos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageArticlesVideos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HomePageArticlesVideos",
                columns: new[] { "Id", "Description", "FilePath", "Title" },
                values: new object[,]
                {
                    { "1", null, null, null },
                    { "2", null, null, null },
                    { "3", null, null, null },
                    { "4", null, null, null },
                    { "5", null, null, null },
                    { "6", null, null, null },
                    { "7", null, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomePageArticlesVideos");
        }
    }
}
