using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WEB2021Apr_P04_T4.Models
{
    public class CompetitionSubmissionViewModel
    {
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }

        [Display(Name = "File submitted")]
        public string? FileSubmitted { get; set; }

        public IFormFile fileToUpload { get; set; }

        [Display(Name = "Date/Time of submission")]
        public DateTime? DateTimeFileUpload { get; set; }

        public string? Appeal { get; set; }

        [Display(Name = "Vote Count")]
        public int VoteCount { get; set; }

        public int? Ranking { get; set; }

        public int TotalScore { get; set; }
    }
}
