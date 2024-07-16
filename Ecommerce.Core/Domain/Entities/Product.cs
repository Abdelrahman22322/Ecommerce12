using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.Entities;

public class Product
{
    [Key]
    public int ProductId { get; set; }

     
    [Required(ErrorMessage = "Product name is required")]
    public string ProductName { get; set; }

    [Required(ErrorMessage = "Supplier ID is required")]
    public int SupplierId { get; set; }

    [Required(ErrorMessage = "Category ID is required")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Brand ID is required")]
    public int BrandId { get; set; }

    [Required(ErrorMessage = "Unit price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Unit price must be a positive value")]
    public decimal UnitPrice { get; set; }

    [Required(ErrorMessage = "Units in stock is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Units in stock must be a non-negative value")]
    public int UnitsInStock { get; set; }

  //  [Required(ErrorMessage = "Units on order is required")]
  //  [Range(0, int.MaxValue, ErrorMessage = "Units on order must be a non-negative value")]
   // public int UnitsOnOrder { get; set; }

    [Required(ErrorMessage = "Reorder level is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Reorder level must be a non-negative value")]
    public int ReorderLevel { get; set; }
    public string ProductCode { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool  IsArchived { get; set; }
    public bool Discontinued { get; set; }
    public bool IsFeatured { get; set; }
    public Supplier Supplier { get; set; }
    public Category Category { get; set; }
    public Brand Brand { get; set; }
    public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<WishlistItem> WishlistItems { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
    public ICollection<ProductCategory> ProductCategories { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; }
    public  Discount Discounts { get; set; }

}
