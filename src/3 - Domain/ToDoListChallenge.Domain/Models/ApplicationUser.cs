using Microsoft.AspNetCore.Identity;

namespace ToDoListChallenge.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeleted { get; set; }
    }
}
