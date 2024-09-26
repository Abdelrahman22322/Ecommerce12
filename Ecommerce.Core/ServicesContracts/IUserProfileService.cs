using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IUserProfileService
{

    //  Task<TDto> AddUserProfileAsync<TDto>(TDto userProfileDto) where TDto : class;
    public Task CreateUserProfileForUserAsync(User user);
    Task<TDto> UpdateUserProfileAsync<TDto>(TDto userProfileDto) where TDto : class;
    Task<UpdateUserProfileDto> GetUserProfileByIdAsync(int id);
    Task UpdateUserProfileDuringCheckoutAsync(CheckoutDto checkoutDto);
}