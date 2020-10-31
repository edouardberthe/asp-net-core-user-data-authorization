using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreUserDataAuthorization.Models;

namespace AspNetCoreUserDataAuthorization.Pages.Contacts
{
    public class IndexModel : PageModel
    {
        private readonly AspNetCoreUserDataAuthorization.Data.AspNetCoreUserDataAuthorizationContext _context;

        public IndexModel(AspNetCoreUserDataAuthorization.Data.AspNetCoreUserDataAuthorizationContext context)
        {
            _context = context;
        }

        public IList<Contact> Contact { get;set; }

        public async Task OnGetAsync()
        {
            Contact = await _context.Contacts.ToListAsync();
        }
    }
}
