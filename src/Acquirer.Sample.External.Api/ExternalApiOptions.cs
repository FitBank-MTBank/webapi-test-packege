namespace Acquirer.Sample.External.Api;

public class ExternalApiOptions
{
    public string Url { get; set; }
    public long PartnerId { get; set; }
    public Uri GetUrl()
        => new(Url!);
}