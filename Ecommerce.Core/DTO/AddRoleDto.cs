using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class AddRoleDto
{
    [Required]
    public int  UserId { get; set; }
    [Required, StringLength(50)]
    public string RoleName { get; set; }

}