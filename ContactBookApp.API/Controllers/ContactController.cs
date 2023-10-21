using ContactBookApp.Core.Services.Implementations;
using ContactBookApp.Core.Services.Interfaces;
using ContactBookApp.Model.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService icontactService)
        {
            _contactService = icontactService;
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return await _contactService.DeleteUserAsync(id);
        }

        [HttpPost("Create-contact")]
        public async Task<IActionResult> AddNewUser([FromBody] ContactDTO model)
        {
            return await _contactService.CreateUserAsync(model);
        }

        [HttpGet("Get-contact-By-Id")]
        public async Task<Response<ContactResponseDTO>> GetUserById(string id)
        {
            return await _contactService.FindUserByIdAsync(id);
        }
    }
}
