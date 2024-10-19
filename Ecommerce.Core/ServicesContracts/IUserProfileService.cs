using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IUserProfileService
{

    //  Task<TDto> AddUserProfileAsync<TDto>(TDto userProfileDto) where TDto : class;
    public Task CreateUserProfileForUserAsync(User user);
   // Task<TDto> UpdateUserProfileAsync<TDto>(TDto userProfileDto) where TDto : class;
    Task<UserProfile> GetUserProfileByIdAsync();
    Task<UserProfileDto> GetUserProfile();
    Task UpdateUserProfileDuringCheckoutAsync(CheckoutDto checkoutDto);

    Task<UserProfileDto> UpdateUserProfileAsync(UpdateUserProfileDto updateUserProfileDto);

    public  Task<UserProfile> GetUserProfileByUserIdAsync(int userId);
}