namespace Ecommerce.Core.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public string Method { get; set; }
    public ICollection<Order> Orders { get; set; }
}