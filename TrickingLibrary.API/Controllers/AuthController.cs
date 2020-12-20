using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrickingLibrary.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId, [FromServices] SignInManager<IdentityUser> signInManager, [FromServices] IIdentityServerInteractionService interactionService)
        {
            await signInManager.SignOutAsync();

            var logoutContext = await interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutContext.PostLogoutRedirectUri))
            {
                return BadRequest();
            }

            return Redirect(logoutContext.PostLogoutRedirectUri);
        }
    }
}
