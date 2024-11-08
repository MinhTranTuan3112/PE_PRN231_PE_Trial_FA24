using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Requests
{
    public class UpdateFootballPlayerRequest
    {
        [RegularExpression(@"^([A-Z][a-zA-Z0-9#]*\s?)+$", ErrorMessage = "Fullname must start each word with a capital letter and contain only letters, digits, spaces, and #.")]
        public string? FullName { get; set; }

        [StringLength(100, MinimumLength = 9, ErrorMessage = "Achievements must be between 9 and 100 characters.")]
        public string? Achievements { get; set; }

        public DateTime? Birthday { get; set; }

        public string? PlayerExperiences { get; set; }

        [StringLength(100, MinimumLength = 9, ErrorMessage = "Nomination must be between 9 and 100 characters.")]
        public string? Nomination { get; set; }

        public string? FootballClubId { get; set; }
    }
}
