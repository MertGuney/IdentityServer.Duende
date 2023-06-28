using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Sex { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public User() { }

        public User(string email, string userName)
        {
            Email = email;
            UserName = userName;
            CreatedDate = DateTime.UtcNow;
            IsActive = true;
            IsDeleted = false;
        }
    }
}
