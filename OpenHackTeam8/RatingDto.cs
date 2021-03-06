﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OpenHackTeam8
{
    public class RatingDto
    {
        public Guid id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public string LocationName { get; set; }
        public int Rating { get; set; }
        public string UserNotes { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
