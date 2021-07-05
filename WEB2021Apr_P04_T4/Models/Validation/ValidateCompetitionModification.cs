using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models.Validation
{
    public class ValidateCompetitionModification : ValidationAttribute
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            int competitionID = Convert.ToInt32(value);
            // Casting the validation context to the "Staff" model class
            Competition competition = (Competition)validationContext.ObjectInstance;
            // Get the Staff Id from the staff instance
            competitionID = competition.CompetitionID;
            if (competitionContext.IsModifiable(competitionID))
                // validation failed
                return new ValidationResult
                ("Competition cannot be modified! A competitior has joined!");
            else
                // validation passed
                return ValidationResult.Success;
        }
    }
}
