using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Model
{
    public class DotNetUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsAllowToUseTheApp { get; set; } = false;
    }
}
