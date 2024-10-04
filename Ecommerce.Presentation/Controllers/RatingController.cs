using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RatingController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpPost]
    public async Task<IActionResult> AddRating([FromBody] RatingDto ratingDto)
    {
        await _ratingService.AddRatingAsync(ratingDto);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRating([FromBody] RatingDto ratingDto)
    {
        await _ratingService.UpdateRatingAsync(ratingDto);
        return Ok();
    }

    [HttpGet("{productId}/total")]
    public async Task<IActionResult> GetTotalRatings(int productId)
    {
        var totalRatings = await _ratingService.GetTotalRatingsForProductAsync(productId);
        return Ok(totalRatings);
    }

    [HttpGet("{productId}/distribution")]
    public async Task<IActionResult> GetRatingDistribution(int productId)
    {
        var distribution = await _ratingService.GetRatingDistributionForProductAsync(productId);
        return Ok(distribution);
    }
}