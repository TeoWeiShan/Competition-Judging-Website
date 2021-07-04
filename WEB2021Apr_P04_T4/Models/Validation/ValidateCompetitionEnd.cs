using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models.Validation
{
    public class ValidateCompetitionEnd : ValidationAttribute
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            DateTime endDate = Convert.ToDateTime(value);
            Competition competition = (Competition)validationContext.ObjectInstance;
            DateTime? startDate = competition.StartDate;
            if (endDate == DateTime.MinValue)
                return ValidationResult.Success;
            else if (endDate < startDate)
                // validation failed
                return new ValidationResult
                ("End Date cannot be earlier than Start Date!");
            else
                // validation passed
                return ValidationResult.Success;
        }

    }
}
