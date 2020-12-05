using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrickingLibrary.API.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginForm Form { get; set; } 

        public void OnGet(string returnUrl)
        {
            Form = new LoginForm { ReturnUrl = returnUrl };
        }

        public async Task<IActionResult> OnPostAsync([FromServices] SignInManager<IdentityUser> signInManager)
        {
            if (!ModelState.IsValid)
                return Page();

            var signInResult = await signInManager.PasswordSignInAsync(Form.UserName, Form.Password, true, false);

            if (signInResult.Succeeded)
            {
                return Redirect(Form.ReturnUrl);
            }

            return Page();
        }

        public class LoginForm
        {
            [Required]
            public string ReturnUrl { get; set; }
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
