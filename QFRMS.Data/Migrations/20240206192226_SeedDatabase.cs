using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QFRMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstituteInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelephoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FocalPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderClassification = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstituteInfo", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1458bc12-081d-4ed7-8243-391e87d6a590", null, "Admin", "ADMIN" },
                    { "687223b0-2022-4d21-8b17-4f506797c568", null, "Trainor", "TRAINOR" },
                    { "e6e46fe3-51d2-4ecf-8d49-81745433a737", null, "Registrar", "REGISTRAR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ExtensionName", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1de02373-df1a-4faf-95a9-cace35127ab6", 0, "14e30be9-6b79-40b1-ac7e-581467dd39e6", null, false, null, "Princess", "Pamplona", true, null, "Payapa", null, "REGISTRAR", "AQAAAAIAAYagAAAAEAVtJEpRo1ZdIcxR/5/odQimBnAPbzIzsONwmoKLxuOnBXiMkNtqXTD6X+Cbrq4sAQ==", null, false, "N4OYNJD57RVF5HR6CN2NOBK3RAF3DFTO", false, "Registrar" },
                    { "2e148a82-d775-4489-a874-116b6cb271a2", 0, "9c530312-7073-4351-8f12-50edc32f5f9a", null, false, null, "Jograd", "Ballesteros", true, null, "Manansala", null, "ADMIN", "AQAAAAIAAYagAAAAEGfqTq21obp9nuyFDRQGnRN+G/OZ/mqK9Ty86iqYaaQmmTdHH98wl5oatjNYRndK0Q==", null, false, "CMNTXYZLDDVS2MPJSYQGBMXUSGFOFHWA", false, "Admin" },
                    { "9f67fc55-0904-4d61-82e6-9c99b61445df", 0, "96b47168-0943-4e66-b67b-bdca05a9b5b6", null, false, "VII", "Fiderico", "Liwanag", true, null, "Antiporda", null, "TRAINOR", "AQAAAAIAAYagAAAAEH0ihb4dMKz25qjUmJ1n+zYrjFFVwjWyxOTTvZ/cifX3n9AbykwbJ3ET1OvY3zFWlw==", null, false, "OSVLNDPJ643I5BC47OBCUPTCIQB3EH7C", false, "Trainor" }
                });

            migrationBuilder.InsertData(
                table: "InstituteInfo",
                columns: new[] { "Id", "Address", "City", "District", "Email", "FocalPerson", "Name", "ProviderClassification", "ProviderType", "Province", "Region", "TelephoneNo" },
                values: new object[] { "1", "Sitio Dao, Brgy. Liloan", "Ormoc City", "IV", "Trainingcenter.Qfarms@gmail.com", "SIMON ANDREW D. QUILANTANG", "QUILANTANG FARM PRODUCTS AND AGRICULTURAL SERVICES", "TVIs", "Private", "Leyte", "Region VIII - Eastern Visayas", "09088661297" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "e6e46fe3-51d2-4ecf-8d49-81745433a737", "1de02373-df1a-4faf-95a9-cace35127ab6" },
                    { "1458bc12-081d-4ed7-8243-391e87d6a590", "2e148a82-d775-4489-a874-116b6cb271a2" },
                    { "687223b0-2022-4d21-8b17-4f506797c568", "9f67fc55-0904-4d61-82e6-9c99b61445df" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstituteInfo");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e6e46fe3-51d2-4ecf-8d49-81745433a737", "1de02373-df1a-4faf-95a9-cace35127ab6" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1458bc12-081d-4ed7-8243-391e87d6a590", "2e148a82-d775-4489-a874-116b6cb271a2" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "687223b0-2022-4d21-8b17-4f506797c568", "9f67fc55-0904-4d61-82e6-9c99b61445df" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1458bc12-081d-4ed7-8243-391e87d6a590");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "687223b0-2022-4d21-8b17-4f506797c568");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6e46fe3-51d2-4ecf-8d49-81745433a737");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1de02373-df1a-4faf-95a9-cace35127ab6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e148a82-d775-4489-a874-116b6cb271a2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9f67fc55-0904-4d61-82e6-9c99b61445df");
        }
    }
}
