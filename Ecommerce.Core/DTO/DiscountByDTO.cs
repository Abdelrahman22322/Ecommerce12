namespace Ecommerce.Core.Domain.RepositoryContracts;

public class DiscountByDTO
{
    public int DiscountId { get; set; }
    public string DiscountName { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    // public List<int> ProductIds { get; set; } = new List<int>();
}