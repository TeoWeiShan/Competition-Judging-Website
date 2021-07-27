using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.Models.Validation;

namespace WEB2021Apr_P04_T4.Models
{
    public class CompetitionJudgeViewModel
    {
        [ValidateCompetitionJudgeModification]
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Competition Name")]
        public string CompetitionName { get; set; }

        [ValidateJudgeDefault]
        [Display(Name = "Judge ID")]
        [Required]
        public int JudgeID { get; set; }

        [Display(Name = "Judge Name")]
        [Required]
        public string JudgeName { get; set; }


    }
}
