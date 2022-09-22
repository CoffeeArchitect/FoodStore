using Microsoft.AspNetCore.Identity;

namespace FoodStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
