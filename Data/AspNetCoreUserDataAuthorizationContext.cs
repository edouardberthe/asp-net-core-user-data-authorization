using AspNetCoreUserDataAuthorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreUserDataAuthorization.Data
{
    public sealed class AspNetCoreUserDataAuthorizationContext : IdentityDbContext<IdentityUser>
    {
        public AspNetCoreUserDataAuthorizationContext(DbContextOptions<AspNetCoreUserDataAuthorizationContext> options)
            : base(options)
        {}

        public DbSet<Contact> Contacts { get; set; }
    }
}