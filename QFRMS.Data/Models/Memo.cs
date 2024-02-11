using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Models
{
    public class Memo
    {
        public required int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateUploaded { get; set; }

        public string? FileId {  get; set; }
        [ForeignKey("FileId"), DeleteBehavior(DeleteBehavior.NoAction)]
        public PDF? File { get; set; }
    }

    public class SeenUsers
    {
        [Key]
        public required string UserId { get; set; }
    }
}
