using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserAccount>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<InstituteInfo> InstituteInfo { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Memo> Memo { get; set; }

        public DbSet<SeenUsers> SeenUsers { get; set; }

        public DbSet<PDF> PDFs { get; set; }

        //Seed Database
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Seeding Database
            //Institute Info
            builder.Entity<InstituteInfo>().HasData(
                new InstituteInfo
                {
                    Id = "1",
                    Name = "QUILANTANG FARM PRODUCTS AND AGRICULTURAL SERVICES",
                    Address = "Sitio Dao, Brgy. Liloan",
                    Region = "Region VIII - Eastern Visayas",
                    Province = "Leyte",
                    District = "IV",
                    City = "Ormoc City",
                    TelephoneNo = "09088661297",
                    Email = "Trainingcenter.Qfarms@gmail.com",
                    FocalPerson = "SIMON ANDREW D. QUILANTANG",
                    ProviderType = "Private",
                    ProviderClassification = "TVIs"
                }
                );

            //Sample Accounts
            builder.Entity<UserAccount>().HasData(
                new UserAccount
                {
                    Id = "1de02373-df1a-4faf-95a9-cace35127ab6",
                    FirstName = "Princess",
                    MiddleName = "Payapa",
                    LastName = "Pamplona",
                    ExtensionName = null,
                    UserName = "Registrar",
                    NormalizedUserName = "REGISTRAR",
                    Email = null,
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEAVtJEpRo1ZdIcxR/5/odQimBnAPbzIzsONwmoKLxuOnBXiMkNtqXTD6X+Cbrq4sAQ==",
                    SecurityStamp = "N4OYNJD57RVF5HR6CN2NOBK3RAF3DFTO",
                    ConcurrencyStamp = "14e30be9-6b79-40b1-ac7e-581467dd39e6",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                },
                new UserAccount
                {
                    Id = "2e148a82-d775-4489-a874-116b6cb271a2",
                    FirstName = "Jograd",
                    MiddleName = "Manansala",
                    LastName = "Ballesteros",
                    ExtensionName = null,
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    Email = null,
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEGfqTq21obp9nuyFDRQGnRN+G/OZ/mqK9Ty86iqYaaQmmTdHH98wl5oatjNYRndK0Q==",
                    SecurityStamp = "CMNTXYZLDDVS2MPJSYQGBMXUSGFOFHWA",
                    ConcurrencyStamp = "9c530312-7073-4351-8f12-50edc32f5f9a",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                },
                new UserAccount
                {
                    Id = "9f67fc55-0904-4d61-82e6-9c99b61445df",
                    FirstName = "Fiderico",
                    MiddleName = "Antiporda",
                    LastName = "Liwanag",
                    ExtensionName = "VII",
                    UserName = "Trainor",
                    NormalizedUserName = "TRAINOR",
                    Email = null,
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEH0ihb4dMKz25qjUmJ1n+zYrjFFVwjWyxOTTvZ/cifX3n9AbykwbJ3ET1OvY3zFWlw==",
                    SecurityStamp = "OSVLNDPJ643I5BC47OBCUPTCIQB3EH7C",
                    ConcurrencyStamp = "96b47168-0943-4e66-b67b-bdca05a9b5b6",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                }
                );

            //User Roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1458bc12-081d-4ed7-8243-391e87d6a590",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = null
                },
                new IdentityRole
                {
                    Id = "687223b0-2022-4d21-8b17-4f506797c568",
                    Name = "Trainor",
                    NormalizedName = "TRAINOR",
                    ConcurrencyStamp = null
                },
                new IdentityRole
                {
                    Id = "e6e46fe3-51d2-4ecf-8d49-81745433a737",
                    Name = "Registrar",
                    NormalizedName = "REGISTRAR",
                    ConcurrencyStamp = null
                }
                );

            //Sample Accounts Roles
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "2e148a82-d775-4489-a874-116b6cb271a2",
                    RoleId = "1458bc12-081d-4ed7-8243-391e87d6a590"
                },
                new IdentityUserRole<string>
                {
                    UserId = "9f67fc55-0904-4d61-82e6-9c99b61445df",
                    RoleId = "687223b0-2022-4d21-8b17-4f506797c568"
                },
                new IdentityUserRole<string>
                {
                    UserId = "1de02373-df1a-4faf-95a9-cace35127ab6",
                    RoleId = "e6e46fe3-51d2-4ecf-8d49-81745433a737"
                }
                );

            builder.Entity<Memo>().HasData(
                new Memo
                {
                    Id = 1
                }
                );
            #endregion
        }
    }
}
