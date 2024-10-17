using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.Entities;

public class Category
{
    
    
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; }

        public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    


    public ICollection<ProductCategory> ProductCategories { get; set; }
}