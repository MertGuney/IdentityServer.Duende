namespace IdentityServer.Infrastructure.Services;
public class RSAKeyService
{
    private readonly TimeSpan _timeSpan;
    private readonly IWebHostEnvironment _environment;
    private string _file => Path.Combine(_environment.WebRootPath, "rsakey.json");

    public RSAKeyService(IWebHostEnvironment environment, TimeSpan timeSpan)
    {
        _environment = environment;
        _timeSpan = timeSpan;
    }

    public bool NeedsUpdate()
    {
        if (File.Exists(_file))
        {
            var creationDate = File.GetCreationTime(_file);
            return DateTime.Now.Subtract(creationDate) > _timeSpan;
        }
        return true;
    }

    public static RSAParameters GetRandomKey()
    {
        using var rsa = new RSACryptoServiceProvider(2048);
        try
        {
            return rsa.ExportParameters(true);
        }
        finally
        {
            rsa.PersistKeyInCsp = false;
        }
    }

    public RSAKeyService GenerateKeyAndSave(bool forceUpdate = false)
    {
        if (forceUpdate || NeedsUpdate())
        {
            var p = GetRandomKey();
            RSAParametersWithPrivate t = new();
            t.SetParameters(p);
            File.WriteAllText(_file, JsonConvert.SerializeObject(t, Formatting.Indented));
        }
        return this;
    }

    public RSAParameters GetKeyParameters()
    {
        if (!File.Exists(_file)) throw new FileNotFoundException("Check configuration - cannot find auth key file: " + _file);
        var keyParams = JsonConvert.DeserializeObject<RSAParametersWithPrivate>(File.ReadAllText(_file));
        return keyParams.ToRSAParameters();
    }

    public RsaSecurityKey GetKey()
    {
        if (NeedsUpdate()) GenerateKeyAndSave();
        var provider = new RSACryptoServiceProvider();
        provider.ImportParameters(GetKeyParameters());
        return new RsaSecurityKey(provider);
    }

    /// <summary>
    /// Util class to allow restoring RSA parameters from JSON as the normal
    /// RSA parameters class won't restore private key info.
    /// </summary>
    private class RSAParametersWithPrivate
    {
        public byte[] D { get; set; }
        public byte[] DP { get; set; }
        public byte[] DQ { get; set; }
        public byte[] Exponent { get; set; }
        public byte[] InverseQ { get; set; }
        public byte[] Modulus { get; set; }
        public byte[] P { get; set; }
        public byte[] Q { get; set; }

        public void SetParameters(RSAParameters p)
        {
            D = p.D;
            DP = p.DP;
            DQ = p.DQ;
            Exponent = p.Exponent;
            InverseQ = p.InverseQ;
            Modulus = p.Modulus;
            P = p.P;
            Q = p.Q;
        }
        public RSAParameters ToRSAParameters()
        {
            return new RSAParameters()
            {
                D = D,
                DP = DP,
                DQ = DQ,
                Exponent = Exponent,
                InverseQ = InverseQ,
                Modulus = Modulus,
                P = P,
                Q = Q
            };
        }
    }
}