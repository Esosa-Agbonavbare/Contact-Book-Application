using ContactBookApp.Core.Services.Implementations;
using ContactBookApp.Core.Services.Interfaces;
using ContactBookApp.Data;
using ContactBookApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ContactBookApp.API.Extension
{
    public static class Startup
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ContactBookContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ContactBookCS"));
            });

            services.Configure<ImageConfiguration>(configuration.GetSection("Cloudinary"));
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImageUploadService, ImageUploadService>();
            services.AddScoped<IContactService, ContactService>();
        }
    }
}