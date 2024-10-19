using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
}