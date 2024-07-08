﻿using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.Entities;

public class WishlistItem
{
    [Key]
    public int WishlistId { get; set; }
    public Wishlist Wishlist { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}