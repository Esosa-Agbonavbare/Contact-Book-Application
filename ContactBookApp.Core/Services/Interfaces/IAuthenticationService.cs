using ContactBookApp.Model.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactBookApp.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterUserAsync(RegistrationDTO model, ModelStateDictionary modelState, string role);
        Task<LogInResponseDTO> LoginAsync(LogInDTO model);
    }
}
