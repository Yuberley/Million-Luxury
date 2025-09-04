namespace MillionLuxury.Infrastructure.Storage.OptionsSetup;

public class MinioOptions
{
    public string Host { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public bool IsSecureSSL { get; init; }
}
