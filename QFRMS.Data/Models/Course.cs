using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Models
{
    public class Course
    {
        public required string Id { get; set; }

        [DisplayName("Program Title : ")]
        [Required(ErrorMessage = "Program Title is required")]
        public required string ProgramTitle { get; set; }

        [DisplayName("Sector : ")]
        [Required(ErrorMessage = "Sector is required")]
        public required string Sector { get; set; }

        [DisplayName("Status : ")]
        [Required(ErrorMessage = "Status is required")]
        public required string Status { get; set; }

        [DisplayName("COPR # : ")]
        [Required(ErrorMessage = "COPR # is required")]
        public required string COPRNo { get; set; }

        [DisplayName("Delivery Mode : ")]
        [Required(ErrorMessage = "Delivery Mode is required")]
        public required string DeliveryMode { get; set; }

        [DisplayName("Duration : ")]
        [Required(ErrorMessage = "Duration is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least an hour.")]
        [RegularExpression(@"([1-9][0-9]*)", ErrorMessage = "Duration must be at least an hour")]
        public required int Duration { get; set; }

        [DisplayName("Classification of Clients : ")]
        [Required(ErrorMessage = "Classification of Clients is required")]
        public required string ClientClassification { get; set; }

        [DisplayName("Type of Scholarship : ")]
        [Required(ErrorMessage = "Type of Scholarship is required")]
        public required string ScholarshipType { get; set; }
    }
}
