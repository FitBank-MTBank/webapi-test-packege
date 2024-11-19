namespace Acquirer.Sample.Domain.Entities;

public class BaseEntity
{
    public bool Excluded { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}
