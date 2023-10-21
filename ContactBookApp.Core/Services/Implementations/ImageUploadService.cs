using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBookApp.Core.Services.Interfaces;
using ContactBookApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ContactBookApp.Core.Services.Implementations
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly UserManager<User> _userManager;
        private readonly Cloudinary _cloudinary;
        
        public ImageUploadService(UserManager<User> userManager, IOptions<ImageConfiguration> cloudinaryConfig)
        {
            _userManager = userManager;

            var account = new Account(
            cloudinaryConfig.Value.CloudName,
            cloudinaryConfig.Value.APIKey,
            cloudinaryConfig.Value.APISecret);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadUserImage(string userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "Invalid data";
            }
            const int MaxFileSize = 300 * 1024;
            if (file.Length > MaxFileSize)
            {
                return "File size exceeds the maximum limit (300kb)";
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return "Only jpg, jpeg, png files are allowed";
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User not found";
            }
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };
            var uploadImage = await _cloudinary.UploadAsync(uploadParams);
            if (uploadImage.Error != null)
            {
                return "Error Uploading image through cloudinary";
            }
            user.ImageURL = uploadImage.Url.ToString();
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return "Failed to update user image";
            }
            return "File updated successfully";
        }
    }
}
