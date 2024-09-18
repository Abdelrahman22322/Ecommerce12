namespace Ecommerce.Core.Domain.Entities;

public class Wishlist
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public User User { get; set; }
    public ICollection<WishlistItem> WishlistItems { get; set; }
}