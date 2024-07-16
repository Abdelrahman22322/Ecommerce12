using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserManagerRepository _userManagerRepository;

    public AuthService(IUserManagerRepository authRepository)
    {
        _userManagerRepository = authRepository;
    }


    public async Task<AuthModel> RegisterAsync(RegisterDto model)
    {
        if (await _userManagerRepository.GetUserByEmailAsync(model.Email) != null)
            return new AuthModel { Message = "Email is already registered" };

        if (await _userManagerRepository.GetUserByUsernameAsync(model.Username) != null)
            return new AuthModel { Message = "Username already exists" };

        var user = await _userManagerRepository.CreateUserAsync(model);

        await _userManagerRepository.AddUserToRoleAsync(user, "User");

        var jwtToken = await _userManagerRepository.GenerateJwtTokenAsync(user);

        return new AuthModel
        {
            Email = user.Email,
            IsAuthentecated = true,
            Roles = new List<string> { "User" },
            Token = jwtToken,
            UserName = user.Username
        };
    }

    public async Task<AuthModel> LoginAsync(LoginModel model)
    {
        var user = await _userManagerRepository.GetUserByEmailAsync(model.Email);

        if (user == null || !await _userManagerRepository.VerifyPasswordAsync(user, model.Password))
            return new AuthModel { Message = "Invalid email or password" };

        var jwtToken = await _userManagerRepository.GenerateJwtTokenAsync(user);
        var roles = await _userManagerRepository.GetUserRolesAsync(user);

        return new AuthModel
        {
            Email = user.Email,
            IsAuthentecated = true,
            Roles = roles.ToList(),
            Token = jwtToken,
            UserName = user.Username
        };
    }

    public async Task<string> AddRoleAsync(AddRoleDto model)
    {
        var user = await _userManagerRepository.GetUserByIdAsync(model.UserId);

        if (user == null)
            return "Invalid user ID";

        var roles = await _userManagerRepository.GetUserRolesAsync(user);
        if (roles.Contains(model.RoleName))
            return "User already assigned to this role";

        await _userManagerRepository.AddUserToRoleAsync(user, model.RoleName);

        return string.Empty;
    }
}