using ContactBookApp.Core.Services.Interfaces;
using ContactBookApp.Data;
using ContactBookApp.Model;
using ContactBookApp.Model.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApp.Core.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly ContactBookContext _contactBookContext;

        public ContactService(ContactBookContext contactBookContext)
        {
            _contactBookContext = contactBookContext;
        }

        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var user = await _contactBookContext.Contacts.FindAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new { Message = "User not found." });
            }

            _contactBookContext.Contacts.Remove(user);
            await _contactBookContext.SaveChangesAsync();
            return new OkObjectResult(new { Message = "User deleted successfully" });
        }

        public async Task<IActionResult> CreateUserAsync(ContactDTO model)
        {
            var existingUser = await _contactBookContext.Contacts.FirstOrDefaultAsync(c => c.Email == model.Email);

            if (existingUser != null)
            {
                return new BadRequestObjectResult(new { Message = "User with the same email already exists" });
            }

            var contact = new Contact
            {
                Id = Guid.NewGuid().ToString(),
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                IsDeleted = false,
            };
            await _contactBookContext.Contacts.AddAsync(contact);
            await _contactBookContext.SaveChangesAsync();

            return new OkObjectResult(new { Message = "User created Successfully" });
        }

        public async Task<Model.DTOs.Response<ContactResponseDTO>> FindUserByIdAsync(string id)
        {
            var response = new Model.DTOs.Response<ContactResponseDTO>();
            var user = await _contactBookContext.Contacts.FindAsync(id);

            if (user == null)
            {
                return response.Failed("User not found", StatusCodes.Status404NotFound);
            }

            var status = new ContactResponseDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id,
                Email = user.Email,

            };
            return response.Success("User found successfully", StatusCodes.Status200OK, status);

        }
    }
}
