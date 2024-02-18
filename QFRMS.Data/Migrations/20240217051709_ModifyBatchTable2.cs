using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QFRMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyBatchTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificatesId",
                table: "Batches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LearningMode",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_CertificatesId",
                table: "Batches",
                column: "CertificatesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_PDFs_CertificatesId",
                table: "Batches",
                column: "CertificatesId",
                principalTable: "PDFs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_PDFs_CertificatesId",
                table: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Batches_CertificatesId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "CertificatesId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "LearningMode",
                table: "Batches");
        }
    }
}
