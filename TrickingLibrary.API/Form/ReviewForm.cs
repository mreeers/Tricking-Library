using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.API.Form
{
    public class ReviewForm
    {
        public int ModerationItemId { get; set; }
        public string Comment { get; set; }
        public ReviewStatus Status { get; set; }
    }
}
