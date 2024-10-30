using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public interface IUserManagerRepository
{



    Task<User> GetUserByUsernameAsync(string username);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByIdAsync(int userId);
    Task<User> CreateUserAsync(RegisterDto registerDto);
    Task<User> UpdateUserAsync(User user);
    Task<User> DeleteUserAsync(int userId);
    Task AddUserToRoleAsync(User user, string rolename);
    Task RemoveUserFromRoleAsync(User user, string role);
    Task<IEnumerable<string>> GetUserRolesAsync(User user);
    Task<string> GenerateJwtTokenAsync(User user);
    Task<bool> VerifyPasswordAsync(User user, string password);
    Task<User> AuthenticateUserAsync(string username, string password);

    Task<PasswordResetToken> GeneratePasswordResetTokenAsync(User user);
    Task<bool> ResetPasswordAsync(User user, string token, string newPassword);


}