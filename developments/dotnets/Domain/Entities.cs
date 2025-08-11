
namespace Domain.Entities;

public class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CustomerId { get; init; }
    public decimal Total { get; private set; }
    public List<OrderItem> Items { get; } = new();

    public void AddItem(string sku, int qty, decimal unitPrice)
    {
        Items.Add(new OrderItem { Sku = sku, Quantity = qty, UnitPrice = unitPrice });
        Recalculate();
    }

    void Recalculate() => Total = Items.Sum(it => it.Quantity * it.UnitPrice);
}

public class OrderItem
{
    public string Sku { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
