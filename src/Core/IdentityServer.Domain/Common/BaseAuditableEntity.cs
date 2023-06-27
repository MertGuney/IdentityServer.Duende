using IdentityServer.Domain.Common.Interfaces;

namespace IdentityServer.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public void AddCreator(string createdBy)
        {
            CreatedBy = createdBy;
            CreatedDate = DateTime.UtcNow;
        }
        public void AddLastModifier(string updatedBy)
        {
            UpdatedBy = updatedBy;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
