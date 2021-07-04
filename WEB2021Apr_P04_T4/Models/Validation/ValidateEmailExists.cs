using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models
{
    public class ValidateEmailExists : ValidationAttribute
    {
        private JudgeProfileDAL judgeContext = new JudgeProfileDAL();
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string emailaddr = Convert.ToString(value);
            // Casting the validation context to the "Judge" model class
            Judge judge = (Judge)validationContext.ObjectInstance;
            // Get the Judge Id from the judge instance
            int judgeID = judge.JudgeID;

            if (judgeContext.IsEmailExist(emailaddr, judgeID))
                // validation failed
                return new ValidationResult
                ("Email address already exists!");
            else
                // validation passed 
                return ValidationResult.Success;
        }
    }
}
