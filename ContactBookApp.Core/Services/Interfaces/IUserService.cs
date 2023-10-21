using ContactBookApp.Model;
using ContactBookApp.Model.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactBookApp.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateNewUserAsync(CreateNewUserDTO model, ModelStateDictionary modelState);
        Task<bool> UpdateUserAsync(string userId, UpdateDTO model);
        Task<PaginationDTO> GetAllUserAsync(int page, int pagesize);
        Task<bool> DeleteUserAsync(string userId);
        Task<User> GetUserByidAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
    }
}
