using System;
using System.Collections.Generic;
using System.Text;

namespace TrickingLibrary.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
    }
}
