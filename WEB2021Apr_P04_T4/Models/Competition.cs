using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.Models.Validation;

namespace WEB2021Apr_P04_T4.Models
{
    public class Competition
    {
        [Display(Name = "Competition ID")]
        [ValidateCompetitionDeletion]
        public int CompetitionID { get; set; }

        [Display(Name = "Interest ID")]
        public int AreaInterestID { get; set; }

        [Display(Name = "Competition Name")]
        [Required]
        [StringLength(255)]
        [ValidateCompetitionExists]
        public string CompetitionName { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Result Released Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ResultReleasedDate { get; set; }
    }
}
