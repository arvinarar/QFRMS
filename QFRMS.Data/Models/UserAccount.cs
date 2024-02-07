using Microsoft.AspNetCore.Identity;

namespace QFRMS.Data.Models
{
    public class UserAccount:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? ExtensionName { get; set; }
    }
}
