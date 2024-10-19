using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class UpdateUserProfileDto
{
   // public int Id { get; set; }

    [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
    public string? FirstName { get; set; }

    [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
    public string? LastName { get; set; }

    [StringLength(100, ErrorMessage = "Street Address cannot be longer than 100 characters.")]
    public string? StreetAddress { get; set; }

    [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters.")]

    public string? City { get; set; }

    [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters.")]
    public string Country { get; set; }

    [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Postal Code must be a valid format (e.g., 12345 or 12345-6789).")]
    public string? PostalCode { get; set; }

    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone Number must be a valid international phone number.")]
    public string? PhoneNumber { get; set; }

    public IFormFile ProfilePicture { get; set; }

}
