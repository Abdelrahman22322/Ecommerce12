namespace Ecommerce.Core.Domain.RepositoryContracts;

public class CommentDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
}