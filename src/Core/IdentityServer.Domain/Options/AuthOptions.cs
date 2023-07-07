namespace IdentityServer.Domain.Options;

public class AuthOptions
{
    public Setting Google { get; set; }
    public Setting Twitter { get; set; }
    public Setting Facebook { get; set; }
}

public class Setting
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}
