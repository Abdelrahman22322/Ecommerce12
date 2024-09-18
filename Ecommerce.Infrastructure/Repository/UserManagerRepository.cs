using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Ecommerce.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository
{
    public class UserManagerRepository : IUserManagerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ICartService _cartService;
        private readonly IWishlistService _wishlistService;

        public UserManagerRepository(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, ICartService cartService, IWishlistService wishlistService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _cartService = cartService;
            _wishlistService = wishlistService;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> CreateUserAsync(RegisterDto registerDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingEmailUser = await GetUserByEmailAsync(registerDto.Email);
                if (existingEmailUser != null)
                    throw new Exception("Email is already registered.");

                var existingUsernameUser = await GetUserByUsernameAsync(registerDto.Username);
                if (existingUsernameUser != null)
                    throw new Exception("Username already exists.");

                if (!IsPasswordComplex(registerDto.Password))
                    throw new Exception("Password does not meet complexity requirements.");

                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    Password = _passwordHasher.HashPassword(null, registerDto.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Create a cart for the new user using CartService
                await _cartService.CreateCartForUserAsync(user);

                // Create a wishlist for the new user using WishlistService
                await _wishlistService.CreateWishlistForUserAsync(user);

                await transaction.CommitAsync();
                return user;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
                return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task AddUserToRoleAsync(User user, string rolename)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == rolename);
            if (role == null)
                throw new Exception("Role does not exist.");

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromRoleAsync(User user, string role)
        {
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.Role.Name == role);
            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(User user)
        {
            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            return roles ?? new List<string>();
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            var userRoles = await GetUserRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
              //  new Claim(ClaimTypes.Role, "user") ,// تأكد من استخدام ClaimTypes.Role
               // new Claim("role","user") ,
                new Claim("uid", user.Id.ToString())
            };

            // Add role claims
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
           // claims.AddRange(userRoles.Select(userRole => new Claim("role", userRole)));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5UUXQCaePfqAWdTJ3yG9+txiWBYwH/VQiQGQiVURJWg="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5190",
                audience: "AAA",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<bool> VerifyPasswordAsync(User user, string password)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return verificationResult == PasswordVerificationResult.Success;
        }

        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        private bool IsPasswordComplex(string password)
        {
            if (password.Length < 8)
                return false;
            if (!password.Any(char.IsUpper))
                return false;
            if (!password.Any(char.IsLower))
                return false;
            if (!password.Any(char.IsDigit))
                return false;
            if (password.All(char.IsLetterOrDigit))
                return false;

            return true;
        }
    }
}
