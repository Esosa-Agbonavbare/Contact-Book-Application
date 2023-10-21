using ContactBookApp.Core.Services.Interfaces;
using ContactBookApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ContactBookApp.Model.DTOs;

namespace ContactBookApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IImageUploadService _imageService;
        private readonly UserManager<User> _userManager;

        public UserController(IUserService userService,IImageUploadService imageService ,UserManager<User> userManager)
        {
            _userService = userService;
            _imageService = imageService;
            _userManager = userManager;
        }

        [HttpPost("add-new-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewUser([FromBody] CreateNewUserDTO model)
        {
            var result = await _userService.CreateNewUserAsync(model, ModelState);
            if (!result)
            {
                return BadRequest(ModelState);
            }
            return Ok(new
            {
                Message = "User created successfully"
            });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateDTO model)
        {
            var userUpdate = await _userService.UpdateUserAsync(id, model);
            if (!userUpdate)
            {
                return BadRequest(new
                {
                    Message = "Update failed"
                });
            }
            return Ok(new
            {
                Message = "User Updated successfully"
            });

        }

        [HttpGet("all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers(int page, int pageSize)
        {
            var paginatedResult = await _userService.GetAllUserAsync(page, pageSize);
            return Ok(paginatedResult);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "User not found"
                });
            }
            return Ok($"User '{user.UserName}' was found ");
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByidAsync(id);
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "User not found"
                });
            }
            return Ok($"User '{user.UserName}' was found ");
        }


        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userDeleted = await _userService.DeleteUserAsync(id);
            if (userDeleted == null)
            {
                return BadRequest(new
                {
                    Message = "User not found or failed to delete user"
                });
            }
            return Ok(new
            {
                Message = "User deleted successfully"
            });
        }

        [HttpPatch("image/{id}")]
        public async Task<IActionResult> UpLoadUserImage(string id, IFormFile image)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Messsage = "User not found" });
            }
            if (image == null)
            {
                return BadRequest(new { Messsage = "image file is required" });
            }
            if (image.Length <= 0)
            {
                return BadRequest(new { Messsage = "image file is empty" });
            }
            var upload = await _imageService.UploadUserImage(id, image);

            if (upload != "File updated successfully")
            {
                return BadRequest(new { Messsage = upload });
            }
            return Ok(new { Messsage = "user image updated successfully" });
        }
    }
}
