using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface ICartService
{
    public  Task CreateCartForUserAsync(User user);
    public Task<Cart> GetCartByUserIdAsync(int userId);
}