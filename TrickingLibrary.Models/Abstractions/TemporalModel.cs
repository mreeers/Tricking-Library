﻿using System.Collections.Generic;
using System.Text;

namespace TrickingLibrary.Models.Abstractions
{
    public abstract class TemporalModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
    }
}
