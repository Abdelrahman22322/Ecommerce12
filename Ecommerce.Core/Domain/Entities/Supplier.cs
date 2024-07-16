namespace Ecommerce.Core.Domain.Entities;

public class Supplier
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string ContactName { get; set; } //  Contact person in the company E.g. John Doe
    public ICollection<Product> Products { get; set; }
}