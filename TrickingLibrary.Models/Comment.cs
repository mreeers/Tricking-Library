using System;
using System.Collections.Generic;
using System.Text;

namespace TrickingLibrary.Models
{
    public class Comment : BaseModel<int>
    {
        public string Content { get; set; }
        public string HtmlContent { get; set; }
    }
}
