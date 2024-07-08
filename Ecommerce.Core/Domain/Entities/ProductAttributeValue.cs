using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.Entities;

public class ProductAttributeValue
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int ProductAttributeId { get; set; }
    public ProductAttribute ProductAttribute { get; set; }
    [Required(ErrorMessage = "Value is required")]
    public string Value { get; set; } // e.g. "Red", "XL", "Cotton"

    
}