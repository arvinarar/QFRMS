using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QFRMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMemo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PDFs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDFs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeenUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeenUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Memo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memo_PDFs_FileId",
                        column: x => x.FileId,
                        principalTable: "PDFs",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Memo",
                columns: new[] { "Id", "DateUploaded", "FileId" },
                values: new object[] { 1, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Memo_FileId",
                table: "Memo",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Memo");

            migrationBuilder.DropTable(
                name: "SeenUsers");

            migrationBuilder.DropTable(
                name: "PDFs");
        }
    }
}
