using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class CreateBrandDto
{
    [Required(ErrorMessage = "Brand name is required")]
    public string Name { get; set; }
    public IFormFile? ImageFile { get; set; }
}