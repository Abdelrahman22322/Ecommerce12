﻿using Ecommerce.Core.Domain.Enums;

namespace Ecommerce.Core.Domain.Entities;

public class OrderStatus
{
    public int Id { get; set; }
    public OrderState Status { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public ICollection<Order> Orders { get; set; }
}