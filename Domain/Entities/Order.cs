namespace Domain.Entities;

public class Order
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long UserId { get; set; }
    public DateTime CreationDate { get; set; }
    public virtual Product Product { get; set; }
    public virtual User Buyer { get; set; }
}