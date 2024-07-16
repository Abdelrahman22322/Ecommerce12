using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class LoginModel
{
    [Required, MaxLength(128)]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
}