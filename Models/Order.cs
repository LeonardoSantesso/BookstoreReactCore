namespace Models;

public class Order 
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Date { get; set; }
    public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
