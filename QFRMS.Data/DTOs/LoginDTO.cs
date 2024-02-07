using System.ComponentModel.DataAnnotations;

namespace QFRMS.Data.DTOs
{
    public class Login
    {
        [Required(ErrorMessage ="Enter Username")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
