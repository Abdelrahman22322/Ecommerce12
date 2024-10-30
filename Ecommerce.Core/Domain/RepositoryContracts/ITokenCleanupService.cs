namespace Ecommerce.Core.Domain.RepositoryContracts;

public interface ITokenCleanupService
{
    Task CleanupExpiredTokensAsync();
}