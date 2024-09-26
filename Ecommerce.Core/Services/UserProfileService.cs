// File: Ecommerce.Core/Services/UserProfileService.cs
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Ecommerce.Core.DTO;

public class UserProfileService : IUserProfileService
{
    private readonly IGenericRepository<UserProfile> _userProfileRepository;
    private readonly IMapper _mapper;

    public UserProfileService(IGenericRepository<UserProfile> userProfileRepository, IMapper mapper)
    {
        _userProfileRepository = userProfileRepository;
        _mapper = mapper;
    }

    //public async Task<TDto> AddUserProfileAsync<TDto>(TDto userProfileDto) where TDto : class
    //{
    //    ValidateDto(userProfileDto);
          
    //    var userProfile = _mapper.Map<UserProfile>(userProfileDto);
    //  //  userProfile.Id = 0;

    //    await _userProfileRepository.AddAsync(userProfile);
    //    await _userProfileRepository.SaveAsync();

    //    return _mapper.Map<TDto>(userProfile);
    //}

    public async Task CreateUserProfileForUserAsync(User user)
    {

        var userProfile = new UserProfile
        {
            UserId = user.Id,
            Usermane = user.Username,
            Email = user.Email,

            // Email = user.Email,
        };

        await _userProfileRepository.AddAsync(userProfile);
        await _userProfileRepository.SaveAsync();
    }

    public async Task<TDto> UpdateUserProfileAsync<TDto>(TDto userProfileDto) where TDto : class
    {
        ValidateDto(userProfileDto);

        var userProfile = await _userProfileRepository.GetByIdAsync((userProfileDto as dynamic).Id);
        if (userProfile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }

        _mapper.Map(userProfileDto, userProfile);

        await _userProfileRepository.UpdateAsync(userProfile);
        await _userProfileRepository.SaveAsync();

        return _mapper.Map<TDto>(userProfile);
    }

    public async Task<UpdateUserProfileDto> GetUserProfileByIdAsync(int id)
    {
        var userProfile = await _userProfileRepository.GetByIdAsync(id);
        if (userProfile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }

        return _mapper.Map<UpdateUserProfileDto>(userProfile);
    }

    public async Task UpdateUserProfileDuringCheckoutAsync(CheckoutDto checkoutDto)
    {
        var userProfile = await _userProfileRepository.GetByIdAsync(checkoutDto.Id);
        if (userProfile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }

        _mapper.Map(checkoutDto, userProfile);

        await _userProfileRepository.UpdateAsync(userProfile);
        await _userProfileRepository.SaveAsync();
    }

    private void ValidateDto<TDto>(TDto dto) where TDto : class
    {
        var validationContext = new ValidationContext(dto);
        Validator.ValidateObject(dto, validationContext, validateAllProperties: true);
    }
}