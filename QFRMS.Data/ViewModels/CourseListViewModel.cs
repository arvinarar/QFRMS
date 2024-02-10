using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.ViewModels
{
    public class CourseListViewModel
    {
        public required string Id { get; set; }
        public required string Sector { get; set; }
        public required string ProgramTitle { get; set; }
        public required string Status { get; set; }
        public required string COPRNo { get; set; }
    }
}
