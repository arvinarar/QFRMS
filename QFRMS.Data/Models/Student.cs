using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QFRMS.Data.Enums.Enums;

namespace QFRMS.Data.Models
{
    public class Student
    {
        [Key]
        public required string ULI { get; set; }

        public required string BatchId { get; set; }

        public required Batch Batch { get; set; }

        public required string FirstName { get; set; }

        public required string MiddleName { get; set; }

        public required string LastName { get; set; }

        public string? ExtensionName { get; set; }

        public string? ContactNo { get; set; }

        public string? Email { get; set; }

        public string? StreetNo { get; set; }

        public required string Barangay { get; set; }

        public required string MunicipalityCity { get; set; }

        public string? District { get; set; }

        public required string Province { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public required Sex Sex { get; set; }

        [DataType(DataType.DateTime)]
        public required DateTime BirthDate { get; set; }

        public required int Age { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public required CivilStatus CivilStatus { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public required HighestGrade HighestGrade { get; set; }

        public required string Nationality { get; set; }


        [Column(TypeName = "nvarchar(24)")]
        public required TrainingStatus TrainingStatus { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public required ESBT ESBT { get; set; }
    }

    public class Grade
    {
        [Key]
        public required string ULI { get; set; }
        [ForeignKey("ULI")]
        public required Student Student { get; set; }

        [Precision(5, 2)]
        public decimal? PreTest { get; set; }
        [Precision(5, 2)]
        public decimal? PostTest { get; set; }
    }

    public enum Sex
    {
        [Description("TBA")]
        TBA,
        [Description("Male")]
        Male,
        [Description("Female")]
        Female,
    }

    public enum CivilStatus
    {
        [Description("TBA")]
        TBA,
        [Description("Single")]
        Single,
        [Description("Married")]
        Married,
        [Description("Divorced")]
        Divorced,
        [Description("Widow/er")]
        Widowed,
    }

    public enum HighestGrade
    {
        [Description("TBA")]
        TBA,
        [Description("Elementary Undergraduate")]
        Elem_U,
        [Description("Elementary Graduate")]
        Elem_G,
        [Description("High School Undergraduate")]
        HS_U,
        [Description("High School Graduate")]
        HS_G,
        [Description("Junior High(K-12)")]
        JuniorHigh,
        [Description("Senior High(K-12)")]
        SeniorHigh,
        [Description("College Undergraduate")]
        College_U,
        [Description("College Graduate")]
        College_G,
    }

    public enum TrainingStatus
    {
        [Description("TBA")]
        TBA,
        [Description("On-going")]
        Ongoing,
        [Description("Completed")]
        Completed,
        [Description("Drop-out")]
        Dropout,
    }

    public enum ESBT
    {
        [Description("TBA")]
        TBA,
        [Description("Unemployed")]
        Unemployed,
        [Description("Self-Employed")]
        SelfEmployed,
        [Description("Wage-Employed")]
        WageEmployed,
    }
}
