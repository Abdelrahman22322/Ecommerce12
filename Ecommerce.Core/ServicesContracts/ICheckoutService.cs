using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface ICheckoutService
{
    Task CheckoutAsync(CheckoutDto checkoutDto ,int userid);
    public Task<UserProfile> GetUserProfileByIdAsync(int userId);
}