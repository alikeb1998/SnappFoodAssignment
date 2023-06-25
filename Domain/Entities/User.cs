namespace Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Order>? Orders { get; set; }
}