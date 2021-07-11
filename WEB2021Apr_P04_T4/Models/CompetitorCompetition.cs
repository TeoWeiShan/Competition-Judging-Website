using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2021Apr_P04_T4.Models.Validation;

namespace WEB2021Apr_P04_T4.Models
{
    public class CompetitorCompetition
    {
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Competition Name")]
        public string CompetitionName { get; set; }

        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }

        [Display(Name = "File Submitted")]
        [StringLength(255)]
        public string? FileSubmitted { get; set; }

        [Display(Name = "File Uploaded")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-DD HH:mm}", ApplyFormatInEditMode = true)]
        [ValidateCompetitionResult]
        public DateTime? DateTimeFileUpload { get; set; }

        [Display(Name = "Appeal")]
        [StringLength(255)]
        public string? Appeal { get; set; }

        [Display(Name = "Vote Count")]
        public int VoteCount { get; set; }

        [Display(Name = "Rank")]
        public int? Ranking { get; set; }
    }
}
