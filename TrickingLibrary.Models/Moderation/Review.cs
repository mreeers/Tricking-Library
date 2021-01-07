﻿using System;
using System.Collections.Generic;
using System.Text;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models.Moderation
{
    public class Review : TemporalModel
    {
        public int ModerationItemId { get; set; }
        public ModerationItem ModerationItem { get; set; }
        public string Comment { get; set; }
        public ReviewStatus Status { get; set; }

    }

    public enum ReviewStatus
    {
        Approved = 0,
        Rejected = 1,
        Waiting = 2
    }
}
