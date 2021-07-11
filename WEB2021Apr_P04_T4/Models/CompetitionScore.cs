using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2021Apr_P04_T4.Models.Validation;

namespace WEB2021Apr_P04_T4.Models
{
    public class CompetitionScore
    {
        [Display(Name = "Criteria ID")]
        public int CriteriaID { get; set; }

        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }

        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Score")]
        public int Score { get; set; }

        [Display(Name = "Last Edited")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-DD HH:mm}", ApplyFormatInEditMode = true)]
        [ValidateCompetitionResult]
        public DateTime? DateTimeLastEdit { get; set; }
    }
}
