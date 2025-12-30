namespace SimplePOS.Application.ProductGroups;

public record CreateProductGroupRequest(string Name);
public record UpdateProductGroupRequest(string Name);
public record ProductGroupResponse(int Id, string Name);