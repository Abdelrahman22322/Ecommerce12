using AutoMapper;

using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

public class RatingService : IRatingService
{
    private readonly IGenericRepository<Rating> _ratingRepository;
    private readonly IMapper _mapper;

    public RatingService(IGenericRepository<Rating> ratingRepository, IMapper mapper)
    {
        _ratingRepository = ratingRepository;
        _mapper = mapper;
    }

    public async Task AddRatingAsync(RatingDto ratingDto)
    {
        var existingRating = await _ratingRepository.FindAsync(r => r.ProductId == ratingDto.ProductId && r.UserId == ratingDto.UserId, null);
        if (existingRating.Any())
        {
            throw new InvalidOperationException("User has already rated this product.");
        }

        var rating = _mapper.Map<Rating>(ratingDto);
        await _ratingRepository.AddAsync(rating);
        await _ratingRepository.SaveAsync();
    }

    public async Task UpdateRatingAsync(RatingDto ratingDto)
    {
        var rating = _mapper.Map<Rating>(ratingDto);
        await _ratingRepository.UpdateAsync(rating);
        await _ratingRepository.SaveAsync();
    }

    public async Task<int> GetTotalRatingsForProductAsync(int productId)
    {
        var ratings = await _ratingRepository.FindAsync(r => r != null && r.ProductId == productId, null);
        return ratings.Count();
    }

    public async Task<Dictionary<int, int>> GetRatingDistributionForProductAsync(int productId)
    {
        var ratings = await _ratingRepository.FindAsync(r => r.ProductId == productId, null);
        return ratings
            .GroupBy(r => r.Value)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}