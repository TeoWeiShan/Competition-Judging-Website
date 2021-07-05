using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P04_T4.Models
{
    public class Competitor
    {
        [Display(Name = "ID")]
        public int CompetitorID { get; set; }

        [Required]
        [StringLength(50)]
        [ValidateInterestExists]
        public string CompetitorName { get; set; }

        [Display(Name = "Salutation")]
        public string Salutation { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DOB { get; set; }

        [Display(Name = "Phone Number")]
        public int PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [StringLength(50)]
        [EmailAddress]
        [ValidateEmailExists]
        public string EmailAddr { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
