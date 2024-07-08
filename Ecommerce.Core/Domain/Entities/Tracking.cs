namespace Ecommerce.Core.Domain.Entities;

public class Tracking
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public string TrackingNumber { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
}