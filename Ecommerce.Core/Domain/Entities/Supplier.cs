namespace Ecommerce.Core.Domain.Entities;

public class Supplier
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public ICollection<Product> Products { get; set; }
}