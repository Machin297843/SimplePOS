namespace SimplePOS.Application.Products;

public record CreateProductRequest(string Sku, string Name, decimal Price, int ProductGroupId);
public record ProductResponse(int Id, string Sku, string Name, decimal Price, int ProductGroupId);