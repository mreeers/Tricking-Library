using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<object> All() => _context.Tricks.Select(TrickViewModels.Default).ToList();

        [HttpGet("{id}")]
        public object Get(string id) => _context.Tricks
            .Where(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
            .Select(TrickViewModels.Default)
            .FirstOrDefault();
        
        [HttpGet("{trickId}/submissions")]
        public IEnumerable<Submission> ListSubmissionsForTricks(string trickId) =>
            _context.Submissions.Where(x => x.TrickId.Equals(trickId, StringComparison.InvariantCultureIgnoreCase)).ToList();

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
            return TrickViewModels.Default.Compile().Invoke(trick);
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
            return TrickViewModels.Default.Compile().Invoke(trick); 
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
