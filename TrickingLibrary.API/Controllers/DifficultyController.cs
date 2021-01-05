using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.API.Controllers
{
    [ApiController]
    [Route("api/difficulties")]
    public class DifficultyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DifficultyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Difficulty> All() => _context.Difficulties.ToList();

        [HttpGet("{id}")]
        public Difficulty Get(string id) => 
            _context.Difficulties.FirstOrDefault(x => x.Slug.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        
        [HttpGet("{id}/tricks")]
        public IEnumerable<Trick> ListdifficultyForTricks(string id) =>
            _context.Tricks
                .Where(x => x.Difficulty.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

        [HttpPost]
        public async Task<Difficulty> Create([FromBody] Difficulty difficulty)
        {
            difficulty.Slug = difficulty.Name.Replace(" ", "-").ToLowerInvariant();
            _context.Add(difficulty);
            await _context.SaveChangesAsync();
            return difficulty;
        }

        
    }
}
