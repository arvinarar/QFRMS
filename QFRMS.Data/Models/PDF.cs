using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Models
{
    public class PDF
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required byte[] File {  get; set; }
    }
}
