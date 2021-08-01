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
        [Required(ErrorMessage = "Please select a criteria to grade.")]
        [Display(Name = "Criteria ID")]
        public int CriteriaID { get; set; }

        [Required(ErrorMessage = "Please enter a CompetitorID to grade.")]
        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }

        [Required(ErrorMessage = "Please select a Competition to grade.")]
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Score")]
        [Range(0, 10, ErrorMessage = "Please enter a score from 0 to 10.")]
        public int Score { get; set; }

    }
}
