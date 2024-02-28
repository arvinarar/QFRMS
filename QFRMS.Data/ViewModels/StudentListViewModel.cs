using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.ViewModels
{
    public class StudentListViewModel
    {
        public required string ULI { get; set; }
        public required string Name { get; set; }
        public required string Age { get; set; }
        public required string ContactNo { get; set; }
        public required string Email { get; set; }
        public required string HighestGrade { get; set; }
        public required string Status { get; set; }
    }
}
