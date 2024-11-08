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
        [Required]
        [RegularExpression(@"^([A-Z][a-zA-Z0-9#]*\s?)+$", ErrorMessage = "Fullname must start each word with a capital letter and contain only letters, digits, spaces, and #.")]
        public required string FullName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 9, ErrorMessage = "Achievements must be between 9 and 100 characters.")]
        public required string Achievements { get; set; }

        [Required]
        public required DateTime Birthday { get; set; }

        [Required]
        public required string PlayerExperiences { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 9, ErrorMessage = "Nomination must be between 9 and 100 characters.")]
        public required string Nomination { get; set; }

        [Required]
        public required string FootballClubId { get; set; }
    }
}
