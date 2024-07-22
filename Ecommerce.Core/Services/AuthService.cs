using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto;
using System.Security.Cryptography;

namespace Ecommerce.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserManagerRepository _userManagerRepository;
    private readonly IEmailService _emailService;
    private readonly Dictionary<string, string> _verificationCodes = new Dictionary<string, string>();


    public AuthService(IUserManagerRepository authRepository, IEmailService emailService)
    {
        _userManagerRepository = authRepository;
        _emailService = emailService;
    }


   

    public async Task<AuthModel> RegisterAsync(RegisterDto model)
    {
        if (await _userManagerRepository.GetUserByEmailAsync(model.Email) != null)
            return new AuthModel { Message = "Email is already registered" };

        if (await _userManagerRepository.GetUserByUsernameAsync(model.Username) != null)
            return new AuthModel { Message = "Username already exists" };

        var user = await _userManagerRepository.CreateUserAsync(model);

        // إرسال كود التحقق
        await SendVerificationCodeAsync(user.Email);

        return new AuthModel
        {
            Email = user.Email,
            IsAuthentecated = false,
            IsVerified = false,
            Message = "Verification code sent. Please verify your email.",
            UserName = user.Username
        };
    }

    public async Task<AuthModel> CompleteRegistrationAsync(string email, string code)
    {
        var user = await _userManagerRepository.GetUserByEmailAsync(email);

        if (user == null)
        {
            return new AuthModel { Message = "Invalid email." };
        }

        if (!await VerifyEmailAsync(email, code))
        {
            return new AuthModel { Message = "Invalid verification code." };
        }

        await _userManagerRepository.AddUserToRoleAsync(user, "User");

        var jwtToken = await _userManagerRepository.GenerateJwtTokenAsync(user);

        return new AuthModel
        {
            Email = user.Email,
            IsAuthentecated = true,
            IsVerified = true,
            Roles = new List<string> { "User" },
            Token = jwtToken,
            UserName = user.Username
        };

    }

    public async Task<AuthModel> LoginAsync(LoginModel model)

    {
         var authmodel = new AuthModel();
        var user = await _userManagerRepository.GetUserByEmailAsync(model.Email);

        if (user == null || !await _userManagerRepository.VerifyPasswordAsync(user, model.Password))
            return new AuthModel { Message = "Invalid email or password" };

        var jwtToken = await _userManagerRepository.GenerateJwtTokenAsync(user);
        var roles = await _userManagerRepository.GetUserRolesAsync(user);


        authmodel.Email = user.Email;
        authmodel.IsAuthentecated = true;
        authmodel.IsVerified = true;
        authmodel.Roles = roles.ToList();
        authmodel.Token = jwtToken;

            

        if (user.RefreshTokens.Any(t => t.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            authmodel.RefreshToken = activeRefreshToken.Token;
            authmodel.RefreshTokenExpireAt = activeRefreshToken.ExpireAt;
        }
        else
        {
            var refreshToken = GenerateRefreshToken();
            authmodel.RefreshToken = refreshToken.Token;
            authmodel.RefreshTokenExpireAt = refreshToken.ExpireAt;
            user.RefreshTokens.Add(refreshToken);
            await _userManagerRepository.UpdateUserAsync(user);
        }
        return authmodel;
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

    public async Task SendVerificationCodeAsync(string email)
    {
        var code = new Random().Next(100000, 999999).ToString();

        if (_verificationCodes.ContainsKey(email))
        {
            _verificationCodes[email] = code;
        }
        else
        {
            _verificationCodes.Add(email, code);
        }

        await _emailService.SendEmailAsync(email, "Verification Code", $"Your verification code is: {code}");
    }

    public Task<bool> VerifyEmailAsync(string email, string code)
    {
        if (_verificationCodes.TryGetValue(email, out var savedCode))
        {
            return Task.FromResult(savedCode == code);
        }
        return Task.FromResult(false);
    }


    private RefreshTokenModel GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var generator = new RNGCryptoServiceProvider();
        generator.GetBytes(randomNumber);

        return new RefreshTokenModel()
        {
            Token = Convert.ToBase64String(randomNumber),

            ExpireAt = DateTime.UtcNow.AddDays(10),

            CreatedAt = DateTime.UtcNow,
        };
    }

}