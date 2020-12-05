﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrickingLibrary.API.ViewModels;
using TrickingLibrary.Data;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.API.Controllers
{
    [Route("api/moderation-items")]
    [ApiController]
    public class ModerationItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModerationItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<ModerationItem> All() => _context.ModerationItems.ToList();

        [HttpGet("{id}")]
        public ModerationItem Get(int id) => _context.ModerationItems.FirstOrDefault(x => x.Id.Equals(id));

        [HttpGet("{id}/comments")]
        public IEnumerable<object> GetComments(int id) =>
            _context.Comments
                .Where(x => x.ModerationItemId.Equals(id))
                .Select(CommentViewModel.Projection)
                .ToList();

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> Comment(int id, [FromBody] Comment comment)
        {
            if (!_context.ModerationItems.Any(x => x.Id == id))
            {
                return NoContent();
            }

            var regex = new Regex(@"\B(?<tag>@[a-zA-Z0-9-_]+)");

            comment.HtmlContent = comment.Content;

            comment.HtmlContent = regex.Matches(comment.Content)
                                       .Aggregate(comment.Content, (content, match) =>
                                        {
                                            var tag = match.Groups["tag"].Value;
                                            return content.Replace(tag, $"<a href=\"{tag}-user-link\">{tag}</a>");
                                        });
            comment.ModerationItemId = id;
            _context.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(CommentViewModel.Create(comment));
        }

        [HttpGet("{id}/reviews")]
        public IEnumerable<Review> GetReviews(int id) =>
            _context.Reviews
                .Where(x => x.ModerationItemId.Equals(id))
                .ToList();

        [HttpPost("{id}/reviews")]
        public async Task<IActionResult> Review(int id, [FromBody] Review review)
        {
            if (!_context.ModerationItems.Any(x => x.Id == id))
            {
                return NoContent();
            }

            review.ModerationItemId = id;
            _context.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }
    }
}
