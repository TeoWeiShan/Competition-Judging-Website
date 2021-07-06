using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2021Apr_P04_T4.Models.Validation;

namespace WEB2021Apr_P04_T4.Models
{
    public class Criteria
    {
        [Display(Name = "Criteria ID")]
        public int CriteriaID { get; set; }

        [Required(ErrorMessage = "Competition cannot be null.")]
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Required(ErrorMessage = "Please enter a criteria.")]
        [Display(Name = "Criteria Name")]
        [ValidateCriteriaExists]
        public string CriteriaName { get; set; }

        [Required(ErrorMessage = "Please enter a weightage.")]
        public int Weightage { get; set; }
    }
}
