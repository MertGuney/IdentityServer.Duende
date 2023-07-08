namespace IdentityServer.Domain.Common.Interfaces;

public interface IAuditableEntity : IEntity
{
    public string CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    void AddCreator(string createdBy);
    void AddLastModifier(string updatedBy);
}
