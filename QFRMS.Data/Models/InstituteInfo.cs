using System.ComponentModel.DataAnnotations;

namespace QFRMS.Data.Models
{
    public class InstituteInfo
    {
        public required string Id { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(256)]
        public required string Address { get; set; 
        }
        [MaxLength(50)]
        public required string Region { get; set; }

        [MaxLength(50)]
        public required string Province { get; set; }

        [MaxLength(50)]
        public required string District { get; set; }

        [MaxLength(50)]
        public required string City { get; set; }

        [MaxLength(20)]
        public required string TelephoneNo { get; set; }

        [MaxLength(100)]
        public required string Email { get; set; }

        [MaxLength(100)]
        public required string FocalPerson { get; set; }

        [MaxLength(50)]
        public required string ProviderType { get; set; }

        [MaxLength(50)]
        public required string ProviderClassification { get; set; }
    }
}
