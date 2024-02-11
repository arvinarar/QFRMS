using System.ComponentModel.DataAnnotations;

namespace QFRMS.Data.Models
{
    public class InstituteInfo
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Region { get; set; }
        public required string Province { get; set; }
        public required string District { get; set; }
        public required string City { get; set; }
        public required string TelephoneNo { get; set; }
        public required string Email { get; set; }
        public required string FocalPerson { get; set; }
        public required string ProviderType { get; set; }
        public required string ProviderClassification { get; set; }
    }
}
