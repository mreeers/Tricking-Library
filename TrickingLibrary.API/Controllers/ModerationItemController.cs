using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrickingLibrary.Data;
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
        public ModerationItem Get(int id) =>
            _context.ModerationItems.FirstOrDefault(x => x.Id.Equals(id));
    }
}
