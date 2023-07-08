namespace IdentityServer.Persistence.Configurations;

public class AspNetUserCodeConfiguration : IEntityTypeConfiguration<AspNetUserCode>
{
    public void Configure(EntityTypeBuilder<AspNetUserCode> builder)
    {
        builder.Property(x => x.Value).HasMaxLength(6).IsRequired();
    }
}
