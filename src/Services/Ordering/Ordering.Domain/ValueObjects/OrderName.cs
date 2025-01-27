namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    private const int DefaultLength = 5;
    public string Value { get; } = default!;

    private OrderName(string value) => Value = value;

    public static OrderName Of(string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNotEqual(DefaultLength, name.Length);

        return new OrderName(name);
    }
}
