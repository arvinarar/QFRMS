using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QFRMS.Data.DTOs
{
    public class CreateBatch
    {
        [DisplayName("Course : ")]
        [Required(ErrorMessage = "Course is required")]
        public string? CourseId { get; set; }
        public List<CourseList>? CourseList { get; set; }

        [DisplayName("Trainor : ")]
        [Required(ErrorMessage = "Trainor is required")]
        public string? TrainorId { get; set; }
        public List<TrainorList>? TrainorList { get; set; }

        [DisplayName("Program Title : ")]
        [Required(ErrorMessage = "Program Title is required")]
        public LearningDelivery? LearningDelivery { get; set; }

        [DisplayName("Learning Method : ")]
        [Required(ErrorMessage = "Learning Method is required")]
        public LearningMode? LearningMode { get; set; }

        [DisplayName("RQM Code : ")]
        [Required(ErrorMessage = "RQM Code is required")]
        [RegularExpression(@"(^([a-zA-Z0-9]{4}[-]?[a-zA-Z0-9]{4}[-]?[a-zA-Z0-9]{4}[-]?[a-zA-Z0-9]{4}[-]?[a-zA-Z0-9]{4})$)", ErrorMessage = "Please enter a valid RQM Code.")]
        public string? RQMNumber { get; set; }

        [DisplayName("Date Start : ")]
        [Required(ErrorMessage = "Date Start is required")]
        [DataType(DataType.Date)]
        public DateTime? DateStart { get; set; }

        [DisplayName("Date Finish : ")]
        [DataType(DataType.Date)]
        public DateTime? DateEnd { get; set; }

        [DisplayName("Time Start : ")]
        [DataType(DataType.Time)]
        public DateTime? TimeStart { get; set; }

        [DisplayName("Time End : ")]
        [DataType(DataType.Time)]
        public DateTime? TimeEnd { get; set; }

        [DisplayName("Employer Name: ")]
        public string? EmployerName { get; set; }

        [DisplayName("Employer Address : ")]
        public string? EmployerAddress { get; set; }

        [DisplayName("Occupation : ")]
        public string? Occupation { get; set; }

        [DisplayName("Classification : ")]
        public string? Classification { get; set; }

        [DisplayName("Salary : ")]
        public string? Salary { get; set; }

        [DisplayName("NTP : ")]
        [Required(ErrorMessage = "Notice to Proceed is required")]
        public IFormFile? NTP { get; set; }

        [DisplayName("Certificates : ")]
        public IFormFile? Certificates { get; set; }

        public required bool FromCoursePage { get; set; }
    }

    public class CourseList
    {
        public string? CourseId { get; set; }
        public string? Name { get; set; }
        public int? Duration { get; set; }
    }

    public class TrainorList
    {
        public string? TrainorId { get; set; }
        public string? Name { get; set; }
    }

    public enum LearningDelivery
    {
        [Description("Program on Accelerating Farm School Establishment (PAFSE)")]
        PAFSE,
    }

    public enum LearningMode
    {
        [Description("Face-to-Face Learning")]
        F2F,

        [Description("Online Learning")]
        Online,

        [Description("Flexible Learning")]
        Flex
    }
}
