﻿using System;
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
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [Display(Name = "Name")]
        public string JudgeName { get; set; }

        public string Salutation { get; set; }

        [Required(ErrorMessage = "Please select an area of interest.")]
        [Display(Name = "Area Of Interest")]
        [ValidateJudgeInterestDefault]
        public int AreaInterestID { get; set; }

        [Required(ErrorMessage = "Please enter an email.")]
        [RegularExpression(@"^[\w-\.]+@([lcu]{3}\.[edu]{3}\.[sg]{2})$",
            ErrorMessage = ("Invalid email format."))]
        [Display(Name = "Email")]
        //Validation attribute to check whether email exists
        [ValidateJudgeEmailExists]
        public string EmailAddr { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$",
                ErrorMessage = ("Your password must contain at least 8 characters with a mix of lower and upper case letters, numerics and special characters."))]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
