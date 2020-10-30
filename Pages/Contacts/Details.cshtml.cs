using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreUserDataAuthorization.Data;
using AspNetCoreUserDataAuthorization.Models;

namespace AspNetCoreUserDataAuthorization.Pages.Contacts
{
    public class DetailsModel : PageModel
    {
        private readonly AspNetCoreUserDataAuthorization.Data.AspNetCoreUserDataAuthorizationContext _context;

        public DetailsModel(AspNetCoreUserDataAuthorization.Data.AspNetCoreUserDataAuthorizationContext context)
        {
            _context = context;
        }

        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contact = await _context.Contacts.FirstOrDefaultAsync(m => m.ContactId == id);

            if (Contact == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
