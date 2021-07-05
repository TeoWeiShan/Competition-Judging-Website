using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P04_T4.Models
{
    public class CompetitionJudge
    {
        [Display(Name = "ID")]
        public int CompetitionID { get; set; }

        [Required]
        public string JudgeID { get; set; }
    }
}
