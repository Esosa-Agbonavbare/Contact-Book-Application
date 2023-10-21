using ContactBookApp.Core.Services.Interfaces;
using ContactBookApp.Model.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO model, string role)
        {
            var registerResult = await _authService.RegisterUserAsync(model, ModelState, role);

            if (!registerResult)
            {
                return BadRequest(new
                    {
                        Message = "Registration failed. Please input a valid role"
                    });
            }
            else
            {
                return Ok(new
                {
                    Message = "User registration successful"
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _authService.LoginAsync(model);
            if (token == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid Credentials"
                });
            }
            return Ok(new
            {
                Response = token
            });

        }
    }
}