using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrickingLibrary.Models;

namespace TrickingLibrary.API.Form
{
    public class TrickForm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}
