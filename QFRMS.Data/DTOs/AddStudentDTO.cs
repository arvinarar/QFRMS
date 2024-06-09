using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using QFRMS.Data.Models;

namespace QFRMS.Data.DTOs
{
    public class EnrollStudent
    {
        // Personal Information
        [DisplayName("First Name : ")]
        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; }

        [DisplayName("Middle Name : ")]
        [Required(ErrorMessage = "Middle Name is required")]
        public string? MiddleName { get; set; }

        [DisplayName("Last Name : ")]
        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; }

        [DisplayName("Extension Name : ")]
        public string? ExtensionName { get; set; }

        [DisplayName("Age : ")]
        [Required(ErrorMessage = "Age is required")]
        public int? Age { get; set; }

        [DisplayName("Birth Date : ")]
        [Required(ErrorMessage = "Birth Date is required")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Sex : ")]
        [Required(ErrorMessage = "Sex is required")]
        public Sex? Sex { get; set; }

        [DisplayName("Civil Status : ")]
        [Required(ErrorMessage = "Civil Status is required")]
        public CivilStatus? CivilStatus { get; set; }

        [DisplayName("Nationality : ")]
        [Required(ErrorMessage = "Nationality is required")]
        public string? Nationality { get; set; }

        //Contact Information
        [DisplayName("Contact No. : ")]
        [RegularExpression(@"(^(09\d{2}?[-]?\d{3}[-]?\d{4})$)", ErrorMessage = "Please enter a valid phone number.")]
        public string? ContactNo { get; set; }

        [DisplayName("Email : ")]
        [EmailAddress(ErrorMessage = "Input valid email")]
        public string? Email { get; set; }

        [DisplayName("Street : ")]
        public string? StreetNo { get; set; }

        [DisplayName("Barangay : ")]
        [Required(ErrorMessage = "Barangay is required")]
        public string? Barangay { get; set; }

        [DisplayName("City/Municipality : ")]
        [Required(ErrorMessage = "City/Municipality is required")]
        public string? MunicipalityCity { get; set; }

        [DisplayName("District : ")]
        public string? District { get; set; }

        [DisplayName("Province : ")]
        [Required(ErrorMessage = "Province is required")]
        public string? Province { get; set; }

        //Learner Information
        [DisplayName("Learner's ID : ")]
        [Required(ErrorMessage = "Learner's ID  is required")]
        [RegularExpression(@"(^([A-Z.]{3}-\d{2}-\d{3}-\d{5}-\d{3})$)", ErrorMessage = "Please enter a valid Learner's Id")]
        public string? ULI { get; set; }

        [DisplayName("Grade Completed : ")]
        [Required(ErrorMessage = "Highest Grade Completed is required")]
        public HighestGrade? HighestGrade { get; set; }

        [DisplayName("Batch Code : ")]
        [Required(ErrorMessage = "Enrolled Batch is required")]
        public string? BatchId { get; set; }
        public List<BatchList>? BatchList { get; set; }

        [DisplayName("Training Status : ")]
        [Required(ErrorMessage = "Training Status is required")]
        public TrainingStatus? TrainingStatus { get; set; }

        [DisplayName("Employment Status before Training : ")]
        [Required(ErrorMessage = "Employment Status before Training is required")]
        public ESBT? ESBT { get; set; }

        public required bool FromStudentsPage { get; set; }
        public required bool FromCoursePage { get; set; }
    }

    public class BatchList
    {
        public required string RQMCode { get; set; }
    }
}
