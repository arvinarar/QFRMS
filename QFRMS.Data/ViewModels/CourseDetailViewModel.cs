using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.ViewModels
{
    public class CourseDetailViewModel : Course
    {
        public required bool CanBeDeleted { get; set; }
    }
}
