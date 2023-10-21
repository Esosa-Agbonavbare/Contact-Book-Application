using ContactBookApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApp.Data
{
    public class ContactBookContext : IdentityDbContext<User>
    {
        public ContactBookContext(DbContextOptions<ContactBookContext> options) : base(options)
        {
            
        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            SeedRoles(builder);

        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
                new IdentityRole() { Name = "Regular", ConcurrencyStamp = "2", NormalizedName = "REGULAR" }
            );
        }
    }
}
