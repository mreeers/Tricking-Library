using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrickingLibrary.Models
{
    public class Trick : BaseModel<string>
    {
        public string Name { get; set; }
        
    }

}
