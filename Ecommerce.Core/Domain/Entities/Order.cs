namespace Ecommerce.Core.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }


    public int OrderStatusId { get; set; }
    public OrderStatus OrderStatus { get; set; }

    public int PaymentId { get; set; }
    public Payment Payment { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public string TrackingNumber { get; set; } 


    //public int ShippingMethodId { get; set; }
    //public string ShippingMethod { get; set; }
    public int ShippingId { get; set; }
    public Shipping Shipping { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }

    public ICollection<Tracking> Trackings { get; set; }

    
    public decimal CalculateTotal()
    {
        decimal subtotal = OrderDetails.Sum(od => od.SubTotal);
        return subtotal + ShippingCost;
    }
}