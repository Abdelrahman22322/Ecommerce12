using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.Entities;

public class Cart
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public User User { get; set; }


    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set;}

    [Required]
    public ICollection<CartItem> CartItems { get; set; }
}

