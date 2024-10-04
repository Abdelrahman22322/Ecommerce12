using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IRatingService
{
    Task AddRatingAsync(RatingDto ratingDto);
    Task UpdateRatingAsync(RatingDto ratingDto);
    Task<int> GetTotalRatingsForProductAsync(int productId);
    Task<Dictionary<int, int>> GetRatingDistributionForProductAsync(int productId);
}