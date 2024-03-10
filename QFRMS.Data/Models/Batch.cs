using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Models
{
    public class Batch
    {
        public required string Id { get; set; }
        public required string CourseId { get; set; }
        public required Course Course { get; set; }

        public required string TrainorId { get; set; }
        public required UserAccount Trainor { get; set; }

        public required string LearningDelivery {  get; set; }

        public required string LearningMode { get; set; }

        public required string RQMNumber { get; set; }

        [DataType(DataType.DateTime)]
        public required DateTime DateStart { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateEnd { get; set;}

        [DataType(DataType.DateTime)]
        public DateTime? TimeStart { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TimeEnd { get; set;}

        public required string NTPId { get; set; }
        [ForeignKey("NTPId"), DeleteBehavior(DeleteBehavior.NoAction)]
        public required PDF NTP { get; set; }

        public string? CertificatesId { get; set; }
        public PDF? Certificates { get; set; }

        public DeploymentDetails? DeploymentDetails { get; set; }

        public ICollection<Student>? Students { get; } = new List<Student>();
    }

    public class DeploymentDetails
    {
        public required string Id { get; set; }
        public required string BatchId { get; set; }
        public required Batch Batch { get; set; }

        public string? EmployerName { get; set; }
        public string? EmployerAddress { get; set; }

        public string? Occupation { get; set; }

        public string? Classification { get; set; }

        public string? Salary { get; set; }
    }
}
