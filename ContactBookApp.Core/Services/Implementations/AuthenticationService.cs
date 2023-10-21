using ContactBookApp.Core.Services.Interfaces;
using ContactBookApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using ContactBookApp.Model.DTOs;

namespace ContactBookApp.Core.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGeneratorService _tokenGenerator;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, ITokenGeneratorService tokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<bool> RegisterUserAsync(RegistrationDTO model, ModelStateDictionary modelState, string role)
        {
            if (!modelState.IsValid)
            {
                return false;
            }
            else
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName

                };

                if (await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            modelState.AddModelError(string.Empty, error.Description);
                        }
                        return false;
                    }
                    await _userManager.AddToRoleAsync(user, role);
                    return true;
                }
                return false;
            }
        }

        public async Task<LogInResponseDTO> LoginAsync(LogInDTO model)
        {
            User user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new LogInResponseDTO
                {
                    ErrorMessage = "User not found"
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new LogInResponseDTO
                {
                    ErrorMessage = "Login failed"
                };
            }
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var token = await _tokenGenerator.GenerateToken(user, role);

            var loginResponse = new LogInResponseDTO
            {
                UserId = user.Id,
                Username = user.UserName,
                Role = role,
                Token = token
            };

            return loginResponse;
        }

    }
}