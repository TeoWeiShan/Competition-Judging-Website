using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P04_T4.Models
{
    public class AreaInterest
    {
        [Display(Name = "ID")]
        public int AreaInterestID { get; set; }

        [Required]
        [StringLength(50)]
        [ValidateInterestExists]
        public string Name { get; set; }

    }
}
