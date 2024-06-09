using System.ComponentModel.DataAnnotations;
using static QFRMS.Data.Enums.Enums;

namespace QFRMS.Data.DTOs
{
    public class UpdateUser
    {
        public required string Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Extension Name")]
        public string? ExtensionName { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        [Display(Name = "Reset Password?")]
        public required bool ResetPassword { get; set; }

        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public UserRoles Role { get; set; }
    }
}
