using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class VerifyEmailDto
{
    public string Email { get; set; }
    public string Code { get; set; }
}

// OrderStatusDto.cs

// OrderDetailDto.cs


// ShippingDto.cs

public class CreateBrandDto
{
    [Required(ErrorMessage = "Brand name is required")]
    public string Name { get; set; }
    public IFormFile? ImageFile { get; set; }
}

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
}
public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Category ID is required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; }

    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
}


public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}