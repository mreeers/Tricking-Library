using System;
using System.Collections.Generic;
using System.Text;

namespace TrickingLibrary.Models
{
    public class Video : BaseModel<int>
    {
        public string VideoLink { get; set; }
        public string ThumbLink { get; set; }
    }
}
