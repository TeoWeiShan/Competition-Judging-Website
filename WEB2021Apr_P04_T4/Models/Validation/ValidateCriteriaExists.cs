using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models.Validation
{
    public class ValidateCriteriaExists : ValidationAttribute
    {
        private CriteriaDAL criteriaContext = new CriteriaDAL();

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            string criteriaName = Convert.ToString(value);
            Criteria criteria = (Criteria)validationContext.ObjectInstance;

            int criteriaID = criteria.CriteriaID;

            if (criteriaContext.IsCriteriaNameExist(criteriaName, criteriaID))
                return new ValidationResult("Criteria already exists!");
            else
                return ValidationResult.Success;
        }
    }
}
