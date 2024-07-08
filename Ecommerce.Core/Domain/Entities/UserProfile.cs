using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.Entities;

public class UserProfile
{
    public int Id { get; set; }


    [Required]

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

}