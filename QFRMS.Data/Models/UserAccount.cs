using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QFRMS.Data.Models
{
    public class UserAccount:IdentityUser
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? MiddleName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(50)]
        public string? ExtensionName { get; set; }
    }
}
