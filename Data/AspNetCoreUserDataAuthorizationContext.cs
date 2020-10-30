using AspNetCoreUserDataAuthorization.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreUserDataAuthorization.Data
{
    public sealed class AspNetCoreUserDataAuthorizationContext : DbContext
    {
        public AspNetCoreUserDataAuthorizationContext(DbContextOptions<AspNetCoreUserDataAuthorizationContext> options)
            : base(options)
        {}

        public DbSet<Contact> Contacts { get; set; }
    }
}