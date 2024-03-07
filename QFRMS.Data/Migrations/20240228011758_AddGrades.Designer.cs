﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QFRMS.Data;

#nullable disable

namespace QFRMS.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240228011758_AddGrades")]
    partial class AddGrades
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1458bc12-081d-4ed7-8243-391e87d6a590",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "687223b0-2022-4d21-8b17-4f506797c568",
                            Name = "Trainor",
                            NormalizedName = "TRAINOR"
                        },
                        new
                        {
                            Id = "e6e46fe3-51d2-4ecf-8d49-81745433a737",
                            Name = "Registrar",
                            NormalizedName = "REGISTRAR"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "2e148a82-d775-4489-a874-116b6cb271a2",
                            RoleId = "1458bc12-081d-4ed7-8243-391e87d6a590"
                        },
                        new
                        {
                            UserId = "9f67fc55-0904-4d61-82e6-9c99b61445df",
                            RoleId = "687223b0-2022-4d21-8b17-4f506797c568"
                        },
                        new
                        {
                            UserId = "1de02373-df1a-4faf-95a9-cace35127ab6",
                            RoleId = "e6e46fe3-51d2-4ecf-8d49-81745433a737"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("QFRMS.Data.Models.Batch", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CertificatesId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("LearningDelivery")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LearningMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NTPId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RQMNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TimeEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TimeStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("TrainorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CertificatesId");

                    b.HasIndex("CourseId");

                    b.HasIndex("NTPId");

                    b.HasIndex("TrainorId");

                    b.ToTable("Batches");
                });

            modelBuilder.Entity("QFRMS.Data.Models.Course", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("COPRNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientClassification")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeliveryMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("ProgramTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScholarshipType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("QFRMS.Data.Models.DeploymentDetails", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BatchId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Classification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployerAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Occupation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salary")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BatchId")
                        .IsUnique();

                    b.ToTable("DeploymentDetails");
                });

            modelBuilder.Entity("QFRMS.Data.Models.Grade", b =>
                {
                    b.Property<string>("ULI")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("PostTest")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal?>("PreTest")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("ULI");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("QFRMS.Data.Models.InstituteInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FocalPerson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderClassification")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelephoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InstituteInfo");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Address = "Sitio Dao, Brgy. Liloan",
                            City = "Ormoc City",
                            District = "IV",
                            Email = "Trainingcenter.Qfarms@gmail.com",
                            FocalPerson = "SIMON ANDREW D. QUILANTANG",
                            Name = "QUILANTANG FARM PRODUCTS AND AGRICULTURAL SERVICES",
                            ProviderClassification = "TVIs",
                            ProviderType = "Private",
                            Province = "Leyte",
                            Region = "Region VIII - Eastern Visayas",
                            TelephoneNo = "09088661297"
                        });
                });

            modelBuilder.Entity("QFRMS.Data.Models.Memo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateUploaded")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("Memo");

                    b.HasData(
                        new
                        {
                            Id = 1
                        });
                });

            modelBuilder.Entity("QFRMS.Data.Models.PDF", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PDFs");
                });

            modelBuilder.Entity("QFRMS.Data.Models.SeenUsers", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.ToTable("SeenUsers");
                });

            modelBuilder.Entity("QFRMS.Data.Models.Student", b =>
                {
                    b.Property<string>("ULI")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Barangay")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BatchId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CivilStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("ContactNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ESBT")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExtensionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HighestGrade")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MunicipalityCity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("StreetNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrainingStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("ULI");

                    b.HasIndex("BatchId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("QFRMS.Data.Models.UserAccount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ExtensionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1de02373-df1a-4faf-95a9-cace35127ab6",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "14e30be9-6b79-40b1-ac7e-581467dd39e6",
                            EmailConfirmed = false,
                            FirstName = "Princess",
                            LastName = "Pamplona",
                            LockoutEnabled = true,
                            MiddleName = "Payapa",
                            NormalizedUserName = "REGISTRAR",
                            PasswordHash = "AQAAAAIAAYagAAAAEAVtJEpRo1ZdIcxR/5/odQimBnAPbzIzsONwmoKLxuOnBXiMkNtqXTD6X+Cbrq4sAQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "N4OYNJD57RVF5HR6CN2NOBK3RAF3DFTO",
                            TwoFactorEnabled = false,
                            UserName = "Registrar"
                        },
                        new
                        {
                            Id = "2e148a82-d775-4489-a874-116b6cb271a2",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "9c530312-7073-4351-8f12-50edc32f5f9a",
                            EmailConfirmed = false,
                            FirstName = "Jograd",
                            LastName = "Ballesteros",
                            LockoutEnabled = true,
                            MiddleName = "Manansala",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAEGfqTq21obp9nuyFDRQGnRN+G/OZ/mqK9Ty86iqYaaQmmTdHH98wl5oatjNYRndK0Q==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "CMNTXYZLDDVS2MPJSYQGBMXUSGFOFHWA",
                            TwoFactorEnabled = false,
                            UserName = "Admin"
                        },
                        new
                        {
                            Id = "9f67fc55-0904-4d61-82e6-9c99b61445df",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "96b47168-0943-4e66-b67b-bdca05a9b5b6",
                            EmailConfirmed = false,
                            ExtensionName = "VII",
                            FirstName = "Fiderico",
                            LastName = "Liwanag",
                            LockoutEnabled = true,
                            MiddleName = "Antiporda",
                            NormalizedUserName = "TRAINOR",
                            PasswordHash = "AQAAAAIAAYagAAAAEH0ihb4dMKz25qjUmJ1n+zYrjFFVwjWyxOTTvZ/cifX3n9AbykwbJ3ET1OvY3zFWlw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "OSVLNDPJ643I5BC47OBCUPTCIQB3EH7C",
                            TwoFactorEnabled = false,
                            UserName = "Trainor"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("QFRMS.Data.Models.UserAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("QFRMS.Data.Models.UserAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QFRMS.Data.Models.UserAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("QFRMS.Data.Models.UserAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QFRMS.Data.Models.Batch", b =>
                {
                    b.HasOne("QFRMS.Data.Models.PDF", "Certificates")
                        .WithMany()
                        .HasForeignKey("CertificatesId");

                    b.HasOne("QFRMS.Data.Models.Course", "Course")
                        .WithMany("Batches")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QFRMS.Data.Models.PDF", "NTP")
                        .WithMany()
                        .HasForeignKey("NTPId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("QFRMS.Data.Models.UserAccount", "Trainor")
                        .WithMany()
                        .HasForeignKey("TrainorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Certificates");

                    b.Navigation("Course");

                    b.Navigation("NTP");

                    b.Navigation("Trainor");
                });

            modelBuilder.Entity("QFRMS.Data.Models.DeploymentDetails", b =>
                {
                    b.HasOne("QFRMS.Data.Models.Batch", "Batch")
                        .WithOne("DeploymentDetails")
                        .HasForeignKey("QFRMS.Data.Models.DeploymentDetails", "BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Batch");
                });

            modelBuilder.Entity("QFRMS.Data.Models.Grade", b =>
                {
                    b.HasOne("QFRMS.Data.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("ULI")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("QFRMS.Data.Models.Memo", b =>
                {
                    b.HasOne("QFRMS.Data.Models.PDF", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("File");
                });

            modelBuilder.Entity("QFRMS.Data.Models.Student", b =>
                {
                    b.HasOne("QFRMS.Data.Models.Batch", "Batch")
                        .WithMany("Students")
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Batch");
                });

            modelBuilder.Entity("QFRMS.Data.Models.Batch", b =>
                {
                    b.Navigation("DeploymentDetails");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("QFRMS.Data.Models.Course", b =>
                {
                    b.Navigation("Batches");
                });
#pragma warning restore 612, 618
        }
    }
}
