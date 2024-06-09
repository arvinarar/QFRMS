using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.ViewModels
{
    public class StudentDetailViewModel
    {
        public required string ULI { get; set; }
        public required string Name { get; set; }
        public required string BirthDate { get; set; }
        public required int Age { get; set; }
        public required string Sex { get; set; }
        public required string CivilStatus { get; set; }
        public required string Nationality { get; set; }
        public string? ContactNo { get; set; }
        public string? Email { get; set; }
        public required string Address { get; set; }
        public required string HighestGrade { get; set; }
        public required string ESBT { get; set; }
        public required string EnrolledProgram { get; set; }
        public required string TrainingStatus { get; set; }
        public string? PreTestGrade { get; set; }
        public string? PostTestGrade { get; set; }
        public string? FinalGrade { get; set; }
        public string? Occupation { get; set; }
        public string? Salary { get; set; }
        public string? Classification { get; set; }
        public string? EmployerName { get; set; }
        public string? EmployerAddress { get; set; }
        public string? BatchId { get; set; }
        public required bool FromStudentsPage { get; set; }
        public required bool FromCoursePage { get; set; }
    }
}