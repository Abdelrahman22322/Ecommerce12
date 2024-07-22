using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterDto model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var result = await _authService.RegisterAsync(model);

        //    if (!result.IsAuthentecated)
        //    {
        //        return BadRequest(result.Message);
        //    }

        //    return Ok(result);
        //}

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var result = await _authService.LoginAsync(model);

        //    if (!result.IsAuthentecated)
        //    {
        //        return BadRequest(result.Message);
        //    }

        //    return Ok(result);
        //}

        //[HttpPost("addRole")]
        //public async Task<IActionResult> AddRole([FromBody] AddRoleDto model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var result = await _authService.AddRoleAsync(model);

        //    if (!string.IsNullOrEmpty(result))
        //    {
        //        return BadRequest(result);
        //    }

        //    return Ok("Role added successfully.");
        //}
        //[HttpPost("send-verification-code")]
        //public async Task<IActionResult> SendVerificationCode([FromBody] string email)
        //{
        //    await _authService.SendVerificationCodeAsync(email);
        //    return Ok("Verification code sent.");
        //}

        //[HttpPost("verify-email")]
        //public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto model)
        //{
        //    var isValid = await _authService.VerifyEmailAsync(model.Email, model.Code);
        //    if (!isValid)
        //        return BadRequest("Invalid verification code.");

        //    return Ok("Email verified.");
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var result = await _authService.RegisterAsync(model);
            if (!result.IsAuthentecated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("complete-registration")]
        public async Task<IActionResult> CompleteRegistration([FromBody] VerificationDto model)
        {
            var result = await _authService.CompleteRegistrationAsync(model.Email, model.Code);
            if (!result.IsAuthentecated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _authService.LoginAsync(model);
            if (!result.IsAuthentecated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(AddRoleDto model)
        {
            var result = await _authService.AddRoleAsync(model);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok();
        }
    }
}
