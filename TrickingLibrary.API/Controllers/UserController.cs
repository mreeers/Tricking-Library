using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.API.Controllers
{
    [Route("api/users")]
    [Authorize(TrickingLibraryConstants.Policies.User)]
    public class UserController : ApiController
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userId = UserId;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));

            if (user != null)
                return Ok(user);

            user = new User
            {
                Id = userId,
                Username = Username
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id) => Ok();

        [HttpGet("{id}/submissions")]
        public Task<List<Submission>> GetUserSubmissions(string id)
        {
            return _context.Submissions
                .Include(x => x.Video)
                .Where(x => x.UserId.Equals(id))
                .ToListAsync();
        }
    }
}
