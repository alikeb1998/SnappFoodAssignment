namespace Domain.Dtos;

public class ProductDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public long InventoryCount { get; set; }
    public decimal Price { get; set; }
    public double Discount { get; set; }
}