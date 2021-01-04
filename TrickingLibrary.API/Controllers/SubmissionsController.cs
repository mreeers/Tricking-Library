using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.API.BackgroundServices.VideoEditing;
using TrickingLibrary.API.Form;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.API.Controllers
{
    [Route("api/submissions")]
    public class SubmissionsController : ApiController
    {
        private readonly AppDbContext _context;

        public SubmissionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Submission> All() => _context.Submissions
            .Where(x => x.VideoProcessed)
            .ToList();

        [HttpGet("{id}")]
        public Submission Get(int id) => _context.Submissions.FirstOrDefault(x => x.Id.Equals(id));
        
        [HttpPost]
        [Authorize(TrickingLibraryConstants.Policies.User)]
        public async Task<IActionResult> Create([FromBody] SubmissionsForm submissionForm, [FromServices] Channel<EditVideoMessage> channel, [FromServices] IFileManager fileManagerLocal)
        {
            if (!fileManagerLocal.TemporaryFileExists(submissionForm.Video))
            {
                return BadRequest();
            }

            var submission = new Submission 
            { 
                TrickId = submissionForm.TrickId,
                Description = submissionForm.Description,
                VideoProcessed = false,
                UserId = UserId
            };
            _context.Add(submission);
            await _context.SaveChangesAsync();
            await channel.Writer.WriteAsync(new EditVideoMessage 
                {
                    SubmissionId = submission.Id,
                    Input = submissionForm.Video,
                });
            return Ok(submission);
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
