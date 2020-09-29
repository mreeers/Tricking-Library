using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrickingLibrary.API.Controllers
{
    [Route("api/tricks")]
    [ApiController]
    public class TricksController : ControllerBase
    {
        private readonly TrickyStore _store;

        public TricksController(TrickyStore store)
        {
            _store = store;
        }

        [HttpGet]
        public IActionResult All() => Ok(_store.All);

        [HttpGet("{id}")]
        public IActionResult Get(int id) => Ok(_store.All.FirstOrDefault(i => i.Id == id));
    }
}
