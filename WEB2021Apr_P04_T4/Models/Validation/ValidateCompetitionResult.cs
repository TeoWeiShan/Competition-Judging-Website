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
            if (value != null)
            {
                DateTime resultDate = Convert.ToDateTime(value);
                Competition competition = (Competition)validationContext.ObjectInstance;
                DateTime? endDate = competition.EndDate;
                if (endDate == null)
                    return new ValidationResult
                    ("End Date is needed to enter values for Result End Date");
                else if (resultDate > DateTime.Today.AddYears(7))
                    return new ValidationResult
                           ("Result Release Date cannot be 7 years later than Today's Date!");
                else if (resultDate == endDate)
                    // validation failed
                    return new ValidationResult
                    ("Result End Date cannot be on the same day as End Date!");
                else if (resultDate < endDate)
                    // validation failed
                    return new ValidationResult
                    ("Result End Date cannot be earlier than End Date!");
                else
                {
                    if (competition.StartDate == null)
                    {
                        return new ValidationResult
                        ("Start Date cannot be null!");
                    }
                    else
                    {
                        // validation passed
                        return ValidationResult.Success;
                    }
                }
                    // validation passed
                    return ValidationResult.Success;
            }
            else { return ValidationResult.Success; }
        }

    }
}
