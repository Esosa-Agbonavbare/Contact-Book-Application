using Microsoft.AspNetCore.Http;

namespace ContactBookApp.Core.Services.Interfaces
{
    public interface IImageUploadService
    {
        Task<string> UploadUserImage(string id, IFormFile image);
    }
}
