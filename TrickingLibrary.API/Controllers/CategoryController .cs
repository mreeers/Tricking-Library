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
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Category> All() => _context.Categories.ToList();

        [HttpGet("{id}")]
        public Category Get(string id) => 
            _context.Categories.FirstOrDefault(x => x.Slug.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        
        [HttpGet("{id}/tricks")]
        public IEnumerable<Trick> ListCategoryTricks(string id) =>
            _context.TrickCategories
                .Include(x => x.Trick)
                .Where(x => x.CategoryId.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Trick)
                .ToList();

        [HttpPost]
        public async Task<Category> Create([FromBody] Category category)
        {
            category.Slug = category.Name.Replace(" ", "-").ToLowerInvariant();
            _context.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        
    }
}
