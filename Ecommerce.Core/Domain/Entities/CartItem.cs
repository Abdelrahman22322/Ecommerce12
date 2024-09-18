using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.Entities;

public class CartItem
{
    
    public int CartId { get; set; }

    [Range(minimum:1 , maximum:10)]
    public int Quantity { get; set; }
    public Cart Cart { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}