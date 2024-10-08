﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Models
{
    public class HomePageArticlesVideo
    {
        public required string Id { get; set; } // Id 1 = video Id, 2-7 are the 6 articles
        [MaxLength(100)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [MaxLength(256)]
        public string? FilePath { get; set; }
    }
}
