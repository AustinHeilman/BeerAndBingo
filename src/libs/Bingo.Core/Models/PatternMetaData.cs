using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.Core.Models
{
    public class PatternMetadata
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Difficulty { get; set; }  // e.g., "Easy", "Hard", "Advanced"
        public bool IsSymmetrical { get; set; }
        public bool IsDefault { get; set; }
    }
}

