using ContactBookApp.Core.Services.Interfaces;
using ContactBookApp.Model;
using ContactBookApp.Model.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace ContactBookApp.Core.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateNewUserAsync(CreateNewUserDTO model, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return false;
            }
            else
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        modelState.AddModelError(string.Empty, error.Description);
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

        public async Task<bool> UpdateUserAsync(string userId, UpdateDTO model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }


        public async Task<PaginationDTO> GetAllUserAsync(int page, int pageSize)
        {
            var totalUsers = await _userManager.Users.CountAsync();
            var totalpages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            page = Math.Max(1, Math.Min(totalpages, page));

            var usersInPage = await _userManager.Users
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var tasks = usersInPage.Select(async user => new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = $"{user.FirstName} {user.LastName}",
                Address = user.Address,
                CreatedAt = user.CreatedAt,
                //Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                ImageURL = user.ImageURL
            });

            var users = await Task.WhenAll(tasks);

            return new PaginationDTO
            {
                TotalUsers = totalUsers,
                CurrentPage = page,
                PageSize = pageSize,
                Users = users.ToList()
            };
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByidAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
