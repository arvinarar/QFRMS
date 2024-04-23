using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.DTOs
{
    public class StudentGrade
    {
        public required string ULI { get; set; }
        public required string Name { get; set; }

        [Range(0, 100, ErrorMessage = "Enter grade between 0-100")]
        [RegularExpression(@"^\d+(\.\d{1,1})?$")]
        public decimal? Pretest { get; set; }

        [Range(0, 100, ErrorMessage = "Enter grade between 0-100")]
        [RegularExpression(@"^\d+(\.\d{1,1})?$")]
        public decimal? Posttest { get; set; }

        public string? FinalGrade { get; set; }

        public required TrainingStatus TrainingStatus { get; set;}
    }

    public class StudentGradesList
    {
        public required bool IsTrainor { get; set; }
        public required bool FromCoursePage { get; set; }
        public required string BatchId { get; set; }
        public required List<StudentGrade> Students { get; set; }
    }
}
