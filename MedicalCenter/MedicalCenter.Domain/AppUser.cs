using Microsoft.AspNetCore.Identity;

namespace MedicalCenter.Domain
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
