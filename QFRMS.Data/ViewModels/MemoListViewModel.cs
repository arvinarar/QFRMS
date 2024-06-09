using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.ViewModels
{
    public class MemoListViewModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string DateUploaded { get; set; }
    }
}
