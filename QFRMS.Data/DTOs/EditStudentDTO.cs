using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.DTOs
{
    public class EditStudent : EnrollStudent
    {
        public required string CurrentBatchId { get; set; }
    }
}
