using ContactBookApp.Model.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApp.Core.Services.Interfaces
{
    public interface IContactService
    {
        Task<IActionResult> DeleteUserAsync(string id);
        Task<IActionResult> CreateUserAsync(ContactDTO model);
        Task<Model.DTOs.Response<ContactResponseDTO>> FindUserByIdAsync(string id);
    }
}
