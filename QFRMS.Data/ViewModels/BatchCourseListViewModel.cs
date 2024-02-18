using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.ViewModels
{
    public class BatchCourseListViewModel
    {
        public required string Id { get; set; }
        public required string RQMCode { get; set; }
        public required string Trainor { get; set; }
        public required string DateStarted { get; set; }
        public required string DateFinished { get; set; }
        public required string LearningMode { get; set; }
    }
}
