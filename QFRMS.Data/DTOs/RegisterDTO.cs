using System.ComponentModel.DataAnnotations;
using static QFRMS.Data.Enums.Enums;

namespace QFRMS.Data.DTOs
{
    public class Register
    {
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
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
        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 8)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public UserRoles userRoles { get; set; }
    }
}
