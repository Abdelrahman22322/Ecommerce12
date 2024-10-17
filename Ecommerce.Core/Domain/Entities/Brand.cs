using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.Entities;

public class Brand
{
    public int Id { get; set; }
    [Required(ErrorMessage = "brand name is required")]
    public string Name { get; set; }
     //public string? Description { get; set; } 
    public ICollection<Product>? Products { get; set; }
    public string? ImageUrl { get; set; }
}