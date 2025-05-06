using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Data
{
    public class AuthDbContext : IdentityDbContext<IdentityUser>
    {
        protected AuthDbContext()
        {
        }
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
