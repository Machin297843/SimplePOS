namespace SimplePOS.Domain.Entities;

public class ProductGroup
{
    public int Id { get; private set; }
    public string Name { get; private set; } = "";

    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private ProductGroup() { } // EF

    public ProductGroup(string name)
    {
        SetName(name);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("ProductGroup name is required.");

        Name = name.Trim();
    }
}