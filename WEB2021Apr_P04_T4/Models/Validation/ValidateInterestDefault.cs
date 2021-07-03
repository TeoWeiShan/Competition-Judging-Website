using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models
{
    public class ValidateInterestDefault :ValidationAttribute
    {
        private CompetitionDAL staffContext = new CompetitionDAL();
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            int areaInterestID= Convert.ToInt32(value);
            // Casting the validation context to the "Staff" model class
            Competition competition = (Competition)validationContext.ObjectInstance;
            // Get the Staff Id from the staff instance
            areaInterestID = competition.AreaInterestID;
            if (areaInterestID == 0)
                return new ValidationResult
                ("Please select a valid area of interest!");
            // validation failed
            
            else
                // validation passed
                return ValidationResult.Success;


        }
    }
}
