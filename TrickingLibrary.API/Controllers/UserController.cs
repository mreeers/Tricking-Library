using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrickingLibrary.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("me")]
        public IActionResult GetMe() => Ok();

        [HttpGet("{id}")]
        public IActionResult GetUser(string id) => Ok();

    }
}
