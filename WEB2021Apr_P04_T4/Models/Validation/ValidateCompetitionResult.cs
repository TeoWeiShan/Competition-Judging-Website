using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models.Validation
{
    public class ValidateCompetitionResult : ValidationAttribute
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            DateTime resultDate = Convert.ToDateTime(value);
            Competition competition = (Competition)validationContext.ObjectInstance;
            DateTime? endDate = competition.EndDate;
            if (resultDate == DateTime.MinValue)
                return ValidationResult.Success;
            else if (resultDate < endDate)
                // validation failed
                return new ValidationResult
                ("Result Date cannot be earlier than End Date!");
            else
                // validation passed
                return ValidationResult.Success;
        }

    }
}
