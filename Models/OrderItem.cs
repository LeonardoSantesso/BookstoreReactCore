namespace Models;

public class OrderItem 
{
    public long Id { get; set; }
    public int Quantity { get; set; } = 0;
    public decimal Total { get; set; } = 0;
    public decimal Price { get; set; } = 0;
    public long OrderId { get; set; }
    public Order Order { get; set; }
}

