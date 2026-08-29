using Microsoft.AspNetCore.Identity;

namespace MovieBookingPro.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
