namespace Ordering.Domain.Models;

public class Product : Entity<ProductId>
{
    public string Name { get; private set; } = default;
    public decimal Price { get; private set; } = default;

    public static Product Create(ProductId id, string name, decimal price)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfLessThan(price, 0);

        return new Product
        {
            Id = id,
            Name = name,
            Price = price,
        };
    }
}
