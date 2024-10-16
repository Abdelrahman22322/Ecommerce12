namespace Ecommerce.Core.Domain.RepositoryContracts;

public class ShipperDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public bool IsDefault { get; set; }
    //public int AssignedOrdersCount { get; set; }
}