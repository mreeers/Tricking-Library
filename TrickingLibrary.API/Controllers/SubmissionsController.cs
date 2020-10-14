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
    [ApiController]
    [Route("api/submissions")]
    public class SubmissionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubmissionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Submission> All() => _context.Submissions.ToList();

        [HttpGet("{id}")]
        public Submission Get(int id) => _context.Submissions.FirstOrDefault(x => x.Id.Equals(id));
        
        [HttpPost]
        public async Task<Submission> Create([FromBody] Submission submission)
        {
            _context.Add(submission);
            await _context.SaveChangesAsync();
            return submission;
        }

        // /api/tricks
        [HttpPut]
        public async Task<Submission> Update([FromBody] Submission submission)
        {
            if(submission.Id == 0)
            {
                return null;
            }
            _context.Add(submission);
            await _context.SaveChangesAsync();
            return submission; 
        }

        // /api/tricks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var submission = _context.Submissions.FirstOrDefault(x => x.Id == id);
            if(submission == null)
            {
                return NotFound();
            }
            submission.Deleted = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
