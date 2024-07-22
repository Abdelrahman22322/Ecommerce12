using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Core.Domain.Entities;

[Owned]
public class RefreshTokenModel
{
    public string Token { get; set; }

    public DateTime ExpireAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime RevokedAt { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpireAt;
    public bool IsActive => RevokedAt == null && !IsExpired;
}