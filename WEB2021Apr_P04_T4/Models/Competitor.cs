using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2021Apr_P04_T4.Models.Validation;

namespace WEB2021Apr_P04_T4.Models
{
    public class Competitor
    {
        [Display(Name = "ID")]
        public int CompetitorID { get; set; }

        [Required]
        [StringLength(50)]
        public string CompetitorName { get; set; }

        [Display(Name = "Salutation")]
        public string Salutation { get; set; }

        [Required(ErrorMessage = "Please enter an email.")]
        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$")]
        [Display(Name = "Email")]
        //Validation attribute to check whether email exists
        [ValidateCompetitorEmail]
        public string EmailAddr { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$",
                ErrorMessage = ("Your password must contain at least 8 characters with a mix of lower and upper case letters, numerics and special characters."))]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
