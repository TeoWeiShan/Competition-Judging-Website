using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models.Validation
{
    public class ValidateCompetitionStart : ValidationAttribute
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            if (value != null)
            {
                DateTime startDate = Convert.ToDateTime(value);
                if (startDate < DateTime.Today)
                    // validation failed
                    return new ValidationResult
                    ("Start Date cannot earlier than Today's Date!");
                else
                    // validation passed
                    return ValidationResult.Success;
            }
            else { return ValidationResult.Success; }
        }

    }
}
