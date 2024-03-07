using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QFRMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPDFTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "PDFs");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "PDFs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "PDFs");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "PDFs",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
