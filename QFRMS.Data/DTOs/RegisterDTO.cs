using System.ComponentModel.DataAnnotations;
using static QFRMS.Data.Enums.Enums;

namespace QFRMS.Data.DTOs
{
    public class Register
    {
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name : ")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Middle Name is required")]
        [Display(Name = "Middle Name : ")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name : ")]
        public string? LastName { get; set; }

        [Display(Name = "Extension Name : ")]
        public string? ExtensionName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username : ")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password : ")]
        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 8)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "Role : ")]
        public UserRoles userRoles { get; set; }
    }
}
