using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Models
{
    public class PDF
    {
        public required string Id { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(256)]
        public required string FilePath {  get; set; }
    }
}
