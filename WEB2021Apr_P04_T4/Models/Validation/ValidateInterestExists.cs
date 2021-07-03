using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models
{
    public class ValidateInterestExists : ValidationAttribute
    {
        private AreaInterestDAL areaInterestContext = new AreaInterestDAL();
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);
            // Casting the validation context to the "Staff" model class
            AreaInterest areaInterest = (AreaInterest)validationContext.ObjectInstance;
            // Get the Staff Id from the staff instance
            int areaInterestID = areaInterest.AreaInterestID;
            if (areaInterestContext.IsInterestExist(email, areaInterestID))
                // validation failed
                return new ValidationResult
                ("Interest already exists!");
            else
                // validation passed
                return ValidationResult.Success;
        }
    }
}
