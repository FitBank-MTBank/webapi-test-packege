namespace Acquirer.Sample.IoC.Options;

public class ResilienceOptions
{
    public bool Enabled { get; set; }
    public int? MaxRetryAttempts { get; set; }
    public int? Timeout { get; set; }
}