using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P04_T4.Models
{
    public class Criteria
    {
        [Display(Name = "Criteria ID")]
        public int CriteriaID { get; set; }

        [Required(ErrorMessage = "Competition ID cannot be null.")]
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Required(ErrorMessage = "Please enter a criteria.")]
        [Display(Name = "Criteria Name")]
        public string CriteriaName { get; set; }

        [Required(ErrorMessage = "Please enter a weightage.")]
        public int Weightage { get; set; }
    }
}
