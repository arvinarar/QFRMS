using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QFRMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyBatchTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_PDFs_NTPId",
                table: "Batches");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_PDFs_NTPId",
                table: "Batches",
                column: "NTPId",
                principalTable: "PDFs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_PDFs_NTPId",
                table: "Batches");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_PDFs_NTPId",
                table: "Batches",
                column: "NTPId",
                principalTable: "PDFs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
