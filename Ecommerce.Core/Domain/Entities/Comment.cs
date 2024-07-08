namespace Ecommerce.Core.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Content { get; set; }
}