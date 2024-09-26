using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

public class CheckoutService : ICheckoutService
{
    private readonly IUserProfileService _userProfileService;
    private readonly IMapper _mapper;

    public CheckoutService(IUserProfileService userProfileService, IMapper mapper)
    {
        _userProfileService = userProfileService;
        _mapper = mapper;
    }

    public async Task CheckoutAsync(CheckoutDto checkoutDto)
    {
        if (checkoutDto == null)
        {
            throw new ArgumentException("User profile data is required for checkout.");
        }

        // Fetch the existing user profile
        var existingUserProfile = await _userProfileService.GetUserProfileByIdAsync(checkoutDto.Id);
        if (existingUserProfile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }

        // Update the existing user profile with the provided data
        _mapper.Map(checkoutDto, existingUserProfile);

        // Validate the updated user profile data
        var validationContext = new ValidationContext(existingUserProfile);
        Validator.ValidateObject(existingUserProfile, validationContext, validateAllProperties: true);

        // Proceed with checkout logic
        // ...

        // Update user profile with provided data
        await _userProfileService.UpdateUserProfileDuringCheckoutAsync(checkoutDto);
    }
}