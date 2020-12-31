using System;
using System.Collections.Generic;
using System.Text;

namespace TrickingLibrary.Models
{
    public class User : BaseModel<string>
    {
        public string Name { get; set; }
    }
}
