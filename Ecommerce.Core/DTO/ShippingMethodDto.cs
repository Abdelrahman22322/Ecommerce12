namespace Ecommerce.Core.Domain.RepositoryContracts;

public class ShippingMethodDto
{
    public int Id { get; set; }
    public string Method { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; } // Added Cost property
}