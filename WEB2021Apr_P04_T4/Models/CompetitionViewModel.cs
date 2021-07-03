using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P04_T4.Models
{
    public class CompetitionViewModel
    {
        [Display(Name = "Competition ID")]
        [ValidateCompetitionExists]
        public int CompetitionID { get; set; }

        [Display(Name = "Interest Name")]
        public int? Name { get; set; }

        [Display(Name = "Competition Name")]
        [Required]
        [StringLength(255)]
        [ValidateCompetitionExists]
        public string CompetitionName { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Result Released Date")]
        public DateTime ResultReleasedDate { get; set; }


    }
}
