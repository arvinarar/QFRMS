using System.ComponentModel.DataAnnotations;
using static QFRMS.Data.Enums.Enums;

namespace QFRMS.Data.DTOs
{
    public class UpdateUserDetails
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public required string Role {  get; set; }

        [Required(ErrorMessage = "Username must not be Empty")]
        [Display(Name = "Username :")]
        public required string Username { get; set; }

        [Display(Name = "Old Password :")]
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }

        [Display(Name = "New Password :")]
        [StringLength(50, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name = "Confirm Password :")]
        [Compare("NewPassword", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
