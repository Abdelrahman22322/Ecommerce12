using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Category ID is required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; }

    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
}