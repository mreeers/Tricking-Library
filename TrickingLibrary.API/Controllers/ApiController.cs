using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrickingLibrary.API.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected string UserId => GetClaim(JwtClaimTypes.Subject);
        protected string Username => GetClaim(JwtClaimTypes.PreferredUserName);

        private string GetClaim(string claimType) => User.Claims.FirstOrDefault(x => x.Type.Equals(claimType))?.Value;
    }
}
