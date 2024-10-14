namespace Ecommerce.Core.Domain.RepositoryContracts;

public class ShippingMethodDto
{
    public int Id { get; set; }
    public string MethodName { get; set; }
    public decimal Cost { get; set; } // Added Cost property
}