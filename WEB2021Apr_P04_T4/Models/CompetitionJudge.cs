using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.Models.Validation;

namespace WEB2021Apr_P04_T4.Models
{
    public class CompetitionJudge
    {
        [ValidateCompetitionJudgeModification]
        [Display(Name = "ID")]
        public int CompetitionID { get; set; }

        [ValidateJudgeDefault]
        [Required]
        public int JudgeID { get; set; }


    }
}
