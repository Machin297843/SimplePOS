namespace SimplePOS.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Sku { get; private set; } = "";
    public string Name { get; private set; } = "";
    public decimal Price { get; private set; }

    public int ProductGroupId { get; private set; }
    public ProductGroup ProductGroup { get; private set; } = null!;

    private Product() { } // EF

    public Product(string sku, string name, decimal price, int productGroupId)
    {
        Update(sku, name, price, productGroupId);
    }

    public void Update(string sku, string name, decimal price, int productGroupId)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU is required.");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");
        if (productGroupId <= 0)
            throw new ArgumentException("ProductGroupId is invalid.");

        Sku = sku.Trim();
        Name = name.Trim();
        Price = price;
        ProductGroupId = productGroupId;
    }
}