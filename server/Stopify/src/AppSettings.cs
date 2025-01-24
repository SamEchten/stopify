namespace Stopify;

public class AppSettings
{
    public required string Key { get; init; }
    public required string Audience { get; init; }
    public required string Issuer { get; init; }
    public required string Salt { get; init; }
}