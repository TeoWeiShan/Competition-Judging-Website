using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models
{
    public class ValidateInterestDeletion : ValidationAttribute
    {
        private AreaInterestDAL areaInterestContext = new AreaInterestDAL();
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            int areaInterestID = Convert.ToInt32(value);
            // Casting the validation context to the "Staff" model class
            AreaInterest areaInterest = (AreaInterest)validationContext.ObjectInstance;
            // Get the Staff Id from the staff instance
            areaInterestID = areaInterest.AreaInterestID;
            if (areaInterestContext.IsDeletable(areaInterestID))
                // validation failed
                return new ValidationResult
                ("Interest cannot be deleted! A competition record has been created under it!");
            else
                // validation passed
                return ValidationResult.Success;
        }
    }
}
