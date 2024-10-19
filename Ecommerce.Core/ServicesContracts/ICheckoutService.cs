using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface ICheckoutService
{
    Task CheckoutAsync(CheckoutDto checkoutDto );
    public Task<UserProfile> GetUserProfileByIdAsync(int userId);
}