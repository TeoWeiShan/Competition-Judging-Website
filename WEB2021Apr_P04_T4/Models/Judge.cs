using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2021Apr_P04_T4.Models
{
    public class Judge
    {
        [Display(Name = "Judge ID")]
        public int JudgeID { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [Display(Name = "Name")]
        public string JudgeName { get; set; }

        public string Salutation { get; set; }

        [Required(ErrorMessage = "Please select an area of interest.")]
        [Display(Name = "Area Of Interest")]
        public int AreaInterestID { get; set; }

        [Required(ErrorMessage = "Please enter an email.")]
        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$")]
        [Display(Name = "Email")]
        public string EmailAddr { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
