using System;
using System.Collections.Generic;
using System.Text;

namespace TrickingLibrary.Models.Moderation
{
    public class ModerationItem : BaseModel<int>
    {
        public string Target { get; set; }
        public string Type { get; set; }
    }
}
