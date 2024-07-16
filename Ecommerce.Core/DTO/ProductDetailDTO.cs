using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.DTO;

public class ProductDetailDTO
{
    
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
     //   public decimal Discount { get; set; }
        
     //   public string? Description { get; set; }
        public string? Supplier { get; set; }
    public string? Brand { get; set; }
        public string? Category { get; set; }
        public int UnitsInStock { get; set; }
      //  public int UnitsOnOrder { get; set; }
     //   public int ReorderLevel { get; set; }
     public int Rating { get; set; }
    public bool Discontinued { get; set; }
      
     
        
      
       
    }


