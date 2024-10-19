using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Ecommerce.Core.DTO;
using Microsoft.AspNetCore.Http;

public class UserProfileService : IUserProfileService
{
    private readonly IGenericRepository<UserProfile> _userProfileRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IImageService _imageService;

    public UserProfileService(IGenericRepository<UserProfile> userProfileRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageService imageService)
    {
        _userProfileRepository = userProfileRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _imageService = imageService;
    }

    public async Task<UserProfileDto> GetUserProfileByTokenAsync()
    {
        var userId = GetUserIdFromContext();
        var userProfile = await _userProfileRepository.FindAsync1(x => x.UserId == userId, null)
                          ?? throw new KeyNotFoundException("User profile not found.");

        return _mapper.Map<UserProfileDto>(userProfile);
    }

    private int GetUserIdFromContext()
    {
        var user = _httpContextAccessor.HttpContext?.User
                   ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "uid")
                          ?? throw new UnauthorizedAccessException("User is not authorized.");

        return int.Parse(userIdClaim.Value);
    }

    public async Task CreateUserProfileForUserAsync(User user)
    {
        var userProfile = new UserProfile
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
        };

        await _userProfileRepository.AddAsync(userProfile);
        await _userProfileRepository.SaveAsync();
    }

    

    public async Task<UserProfileDto> UpdateUserProfileAsync(UpdateUserProfileDto updateUserProfileDto)
    {
        ValidateDto(updateUserProfileDto);

        var userProfile = await GetUserProfileByIdAsync();
        _mapper.Map(updateUserProfileDto, userProfile);

        if (updateUserProfileDto.ProfilePicture != null)
        {
            var uploadResult = await _imageService.UploadImageAsync(updateUserProfileDto.ProfilePicture);
            userProfile.ProfilePictureUrl = uploadResult.Url.ToString();
        }

        await _userProfileRepository.UpdateAsync(userProfile);
        await _userProfileRepository.SaveAsync();

        return _mapper.Map<UserProfileDto>(userProfile);
    }

    public async Task<UserProfile> GetUserProfileByIdAsync()
    {
        var userId = GetUserIdFromContext();
        return await _userProfileRepository.FindAsync1(x => x.UserId == userId, null)
               ?? throw new KeyNotFoundException("User profile not found.");
    }

    public async Task<UserProfileDto> GetUserProfile()
    {

        var userProfile = await GetUserProfileByIdAsync();
        return _mapper.Map<UserProfileDto>(userProfile);
    }

    public async Task UpdateUserProfileDuringCheckoutAsync(CheckoutDto checkoutDto)
    {
        var userProfile = await GetUserProfileByIdAsync();
        _mapper.Map(checkoutDto, userProfile);

        await _userProfileRepository.UpdateAsync(userProfile);
        await _userProfileRepository.SaveAsync();
    }

    private void ValidateDto<TDto>(TDto dto) where TDto : class
    {
        var validationContext = new ValidationContext(dto);
        Validator.ValidateObject(dto, validationContext, validateAllProperties: true);
    }

    public async Task<UserProfile> GetUserProfileByUserIdAsync(int userId)
    {
        return await _userProfileRepository.FindAsync1(x => x.UserId == userId, null)
               ?? throw new KeyNotFoundException("User profile not found.");
    }
}
