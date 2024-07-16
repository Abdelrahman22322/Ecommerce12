using Microsoft.AspNetCore.Http;

namespace Ecommerce.Core.DTO;

public class AddProductDto
{

    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public string Description { get; set; }
    public  string Brand { get; set; }
    public string Category { get; set; }
    public int UnitsInStock { get; set; }
 //   public int UnitsOnOrder { get; set; }
    public int ReorderLevel { get; set; }
    public bool Discontinued { get; set; }
    public bool IsArchived { get; set; }
    public string ProductCode { get; set; } 
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public List<string> Tags { get; set; }
    public List<string> ProductAttributes { get; set; }
    public List<string> ProductAttributesValues { get; set; }
    // public List<string> ProductImageUrls { get; set; }
    public List<IFormFile> ProductImages { get; set; }
    public List<string> ImageUrls { get; set; }
  //   public int Rating { get; set; }
    public int  DiscountAmount { get; set; }
    public DateTime DiscountStartDate { get; set; } = DateTime.Now;
    public DateTime DiscountEndDate { get; set; } 

}