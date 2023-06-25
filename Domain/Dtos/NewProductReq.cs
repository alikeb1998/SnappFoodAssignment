namespace Domain.Dtos;

public class NewProductReq
{
    public string Title { get; set; }
    public long InventoryCount { get; set; }
    public decimal Price { get; set; }
    public double Discount { get; set; }
}