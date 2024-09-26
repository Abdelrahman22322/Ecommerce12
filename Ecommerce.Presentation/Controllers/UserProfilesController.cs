using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IMapper _mapper;

        public UserProfilesController(IUserProfileService userProfileService, IMapper mapper)
        {
            _userProfileService = userProfileService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            try
            {
                var userProfileDto = await _userProfileService.GetUserProfileByIdAsync(id);
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] JsonPatchDocument<UpdateUserProfileDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest(new { message = "Invalid patch document." });
            }

            try
            {
                var userProfileDto = await _userProfileService.GetUserProfileByIdAsync(id);
                patchDoc.ApplyTo(userProfileDto, (error) =>
                {
                    throw new InvalidOperationException($"Error applying patch: {error.ErrorMessage}");
                });

                var updatedUserProfile = await _userProfileService.UpdateUserProfileAsync(userProfileDto);
                return Ok(updatedUserProfile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
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