using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QFRMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyBatchTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStart",
                table: "Batches",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeEnd",
                table: "Batches",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NTPId",
                table: "Batches",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_NTPId",
                table: "Batches",
                column: "NTPId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_PDFs_NTPId",
                table: "Batches",
                column: "NTPId",
                principalTable: "PDFs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_PDFs_NTPId",
                table: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Batches_NTPId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "NTPId",
                table: "Batches");

            migrationBuilder.AlterColumn<string>(
                name: "TimeStart",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TimeEnd",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
