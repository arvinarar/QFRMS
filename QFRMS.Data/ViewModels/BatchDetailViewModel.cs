using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.ViewModels
{
    public class BatchDetailViewModel
    {
        public required string Id { get; set; }
        public required string ProgramTitle { get; set; }
        public required string RQMCode { get; set; }
        public required string TrainorName { get; set; }
        public required string Session { get; set; }
        public required string LearningMode { get; set; }
        public required string LearningDelivery { get; set; }
        public required string Schedule { get; set; }
        public required string NTPId { get; set; }
        public string? CertificatesId { get; set; }
        public required bool CanBeDeleted { get; set; }
        public string? CourseId { get; set; }
    }
}
