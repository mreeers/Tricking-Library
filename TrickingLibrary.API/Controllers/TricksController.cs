﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.API.Controllers
{
    [Route("api/tricks")]
    [ApiController]
    public class TricksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TricksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Trick> All() => _context.Tricks.ToList();

        [HttpGet("{id}")]
        public Trick Get(int id) => _context.Tricks.FirstOrDefault(x => x.Id.Equals(id));
        
        [HttpGet("{trickId}/submissions")]
        public IEnumerable<Submission> ListSubmissionsForTrick(int trickId) =>
            _context.Submissions.Where(x => x.TrickId.Equals(trickId)).ToList();

        [HttpPost]
        public async Task<Trick> Create([FromBody] Trick trick)
        {
            _context.Add(trick);
            await _context.SaveChangesAsync();
            return trick;
        }

        // /api/tricks
        [HttpPut]
        public async Task<Trick> Update([FromBody] Trick trick)
        {
            if(trick.Id == 0)
            {
                return null;
            }
            _context.Add(trick);
            await _context.SaveChangesAsync();
            return trick; 
        }

        // /api/tricks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
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
