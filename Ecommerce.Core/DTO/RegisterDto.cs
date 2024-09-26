using System.ComponentModel.DataAnnotations;



public class RegisterDto
{
    [Required, MaxLength(50)]
    
    public string Username { get; set; }
  //  [Required, MaxLength(128), DataType(dataType: DataType.EmailAddress)]
    public string Email { get; set; }
    [Required, MaxLength(128), DataType(dataType: DataType.Password)]
    public string Password { get; set; }

    [Required, MaxLength(128), DataType(dataType: DataType.Password), Compare("Password")]
    public string ConfirmPassword { get; set; }


}