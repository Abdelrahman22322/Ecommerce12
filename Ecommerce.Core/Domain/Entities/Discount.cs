﻿using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.Entities;

public class Discount
{
    public int DiscountId { get; set; }
    [Required]
    public required string DiscountName { get; set; }
    [Required]
    public decimal DiscountAmount { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
   
    
    public ICollection<Product> Products { get; set; }
}