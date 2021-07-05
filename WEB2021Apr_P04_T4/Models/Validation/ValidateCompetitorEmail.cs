using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models.Validation
{
    public class ValidateCompetitorEmail : ValidationAttribute
    {
        private CompetitorDAL competitorContext = new CompetitorDAL();
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);
            // Casting the validation context to the "Staff" model class
            Competitor competitor = (Competitor)validationContext.ObjectInstance;
            // Get the Staff Id from the staff instance
            int competitorId = competitor.CompetitorID;
            if (competitorContext.IsEmailExist(email, competitorId))
                // validation failed
                return new ValidationResult
                ("Email address already exists!");
            else
                // validation passed
                return ValidationResult.Success;
        }
    }
}
