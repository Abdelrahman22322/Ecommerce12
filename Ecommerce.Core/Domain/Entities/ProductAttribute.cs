namespace Ecommerce.Core.Domain.Entities;

public class ProductAttribute
{
    public int Id { get; set; }
    public string Name { get; set; } // e.g. "Color", "Size", "Material"
    public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } 
}