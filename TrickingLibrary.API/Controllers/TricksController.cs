using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.API.Form;
using TrickingLibrary.API.ViewModels;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.API.Controllers
{
    [ApiController]
    [Route("api/tricks")]
    public class TricksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TricksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<object> All() => _context.Tricks.Select(TrickViewModels.Projection).ToList();

        [HttpGet("test")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
        public string TestAuth() => "test";

        [HttpGet("mod")]
        [Authorize(Policy = TrickingLibraryConstants.Policies.Mod)]
        public string ModAuth() => "mod";

        [HttpGet("{id}")]
        public object Get(string id) => _context.Tricks
            .Where(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
            .Select(TrickViewModels.Projection)
            .FirstOrDefault();
        
        [HttpGet("{trickId}/submissions")]
        public IEnumerable<Submission> ListSubmissionsForTricks(string trickId) =>
            _context.Submissions
            .Include(x => x.Video)
            .Where(x => x.TrickId.Equals(trickId, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        [HttpPost]
        public async Task<object> Create([FromBody] TrickForm trickForm)
        {
            var trick = new Trick
            {
                Id = trickForm.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = trickForm.Name,
                Description = trickForm.Description,
                Difficulty = trickForm.Difficulty,
                TrickCategories = trickForm.Categories.Select(x => new TrickCategory
                {
                    CategoryId = x
                }).ToList()
            };
            _context.Add(trick);
            await _context.SaveChangesAsync();
            return TrickViewModels.Create(trick);
        }

        [HttpPut]
        public async Task<object> Update([FromBody] Trick trick)
        {
            if(string.IsNullOrEmpty(trick.Id))
            {
                return null;
            }
            _context.Add(trick);
            await _context.SaveChangesAsync();
            return TrickViewModels.Create(trick); 
        }

        // /api/tricks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var trick = _context.Tricks.FirstOrDefault(x => x.Id == id);
            if (trick == null)
            {
                return NotFound();
            }
            trick.Deleted = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
