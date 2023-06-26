using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Sex { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? IdentityNumber { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
