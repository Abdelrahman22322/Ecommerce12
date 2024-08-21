namespace Ecommerce.Core.Domain.RepositoryContracts;

 public class DiscountDTO
{
    
    public decimal DiscountAmount { get; set; }
   
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ProductId { get; set; }

}