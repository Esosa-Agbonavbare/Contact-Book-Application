using Microsoft.AspNetCore.Identity;

namespace ContactBookApp.Model
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageURL { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Contact> Contacts { get; set; }
    }
}
