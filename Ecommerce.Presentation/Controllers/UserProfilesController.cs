using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ecommerce.Core.Domain.RepositoryContracts;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "UserPolicy")]

    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IMapper _mapper;


        public UserProfilesController(IUserProfileService userProfileService, IMapper mapper)
        {
            _userProfileService = userProfileService;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetUserProfileById()
        {
            try
            {
                var userProfileDto = await _userProfileService.GetUserProfile();
                return Ok(userProfileDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> AddUserProfile([FromBody] UserProfileDto userProfileDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var createdUserProfile = await _userProfileService.AddUserProfileAsync(userProfileDto);
        //        return CreatedAtAction(nameof(GetUserProfileById), new { id = createdUserProfile.Id }, createdUserProfile);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        //    }
        //}

        //[HttpPatch("Update")]
        //public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileDto updateUserProfileDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var userProfileDto = await _userProfileService.GetUserProfileByIdAsync();

        //        var properties = typeof(UpdateUserProfileDto).GetProperties();
        //        foreach (var property in properties)
        //        {
        //            var newValue = property.GetValue(updateUserProfileDto);
        //            if (newValue != null)
        //            {
        //                var userProfileProperty = userProfileDto.GetType().GetProperty(property.Name);
        //                if (userProfileProperty != null)
        //                {
        //                    userProfileProperty.SetValue(userProfileDto, newValue);
        //                }
        //            }
        //        }

        //        var updatedUserProfile = await _userProfileService.UpdateUserProfileAsync(userProfileDto);
        //        return Ok(updatedUserProfile);
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        //    }
        //}

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileDto updateUserProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedUserProfile = await _userProfileService.UpdateUserProfileAsync(updateUserProfileDto);
                return Ok(updatedUserProfile);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }
    }
}