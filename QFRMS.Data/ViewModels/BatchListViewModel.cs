using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.ViewModels
{
    public class BatchListViewModel
    {
        public required string Id { get; set; }
        public required string RQMCode { get; set; }
        public required string ProgramTitle { get; set; }
        public required string TrainorName { get; set; }
        public required string Period {  get; set; }
    }
}
