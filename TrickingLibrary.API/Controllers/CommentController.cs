using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrickingLibrary.API.ViewModels;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}/replies")]
        public IEnumerable<object> GetReplies(int id) => 
            _context.Comments
            .Where(x => x.ParentId == id)
            .Select(CommentViewModel.Projection)
            .ToList();

        [HttpPost("{id}/replies")]
        public async Task<IActionResult> Reply(int id, [FromBody] Comment reply)
        {
            var comment = _context.Comments.FirstOrDefault(x => x.Id == id);

            if(comment == null)
            {
                return NoContent();
            }

            var regex = new Regex(@"\B(?<tag>@[a-zA-Z0-9-_]+)");

            reply.HtmlContent = regex.Matches(reply.Content)
                                       .Aggregate(reply.Content, (content, match) =>
                                       {
                                           var tag = match.Groups["tag"].Value;
                                           return content.Replace(tag, $"<a href=\"{tag}-user-link\">{tag}</a>");
                                       });

            comment.Replies.Add(reply);
            await _context.SaveChangesAsync();

            return Ok(CommentViewModel.Create(reply));
        }
    }
}
