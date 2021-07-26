using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P04_T4.Models
{
    public class CompetitionSubmission
    {
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }

        [Display(Name = "File submitted")]
        public char FileSubmitted { get; set; }

        [Display(Name = "Date/Time of submission")]
        public DateTime DateTimeFileUpload { get; set; }

        public char Appeal { get; set; }

        [Display(Name = "Vote Count")]
        public int VoteCount { get; set; }

        public int Ranking { get; set; }
    }
}
