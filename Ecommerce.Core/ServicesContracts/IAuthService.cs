using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IAuthService
{
    public Task<AuthModel> RegisterAsync(RegisterDto model);

    public Task<AuthModel> LoginAsync(LoginModel model);

    Task<string> AddRoleAsync(AddRoleDto model);
   
    
        Task SendVerificationCodeAsync(string email);
        Task<bool> VerifyEmailAsync(string email, string code);
        Task<AuthModel> CompleteRegistrationAsync(string email, string code);


        Task<string> ForgetPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);

}