using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Domain.Entities
{
    public class User
    {
        [Key]

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your username")]       
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 20 characters")]
        public string Username { get; set; }
         
        [Required(ErrorMessage = "Please enter your email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
         
      
       
        [StringLength(256)]
        public string Password { get; set; }
        
        //[Required(ErrorMessage = "Please confirm your password")]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "Password does not match")]
        //public string ConfirmPassword { get; set; }
        

        public bool IsActive { get; set; }
        //public bool IsVerified { get; set; }


        public ICollection<UserRole> UserRoles { get; set; }
        public UserProfile UserProfile { get; set; }
        public Cart Cart { get; set; }
        public Wishlist Wishlist { get; set; }

        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Comment>  Comments { get; set; } 
        public ICollection<RefreshTokenModel> RefreshTokens { get; set; }


    }
}
