﻿namespace Ecommerce.Core.Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string ImageUrl { get; set; }
    public string PublicId { get; set; }
}