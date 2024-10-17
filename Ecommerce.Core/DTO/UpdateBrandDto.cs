using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class UpdateBrandDto
{
    [Required(ErrorMessage = "Brand ID is required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Brand name is required")]
    public string Name { get; set; }
    public IFormFile? ImageFile { get; set; }
}