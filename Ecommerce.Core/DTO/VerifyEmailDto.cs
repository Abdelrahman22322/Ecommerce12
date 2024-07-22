namespace Ecommerce.Core.Domain.RepositoryContracts;

public class VerifyEmailDto
{
    public string Email { get; set; }
    public string Code { get; set; }
}