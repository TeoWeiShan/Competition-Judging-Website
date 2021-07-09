using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models.Validation
{
    public class ValidateCriteriaWeightage100 : ValidationAttribute
    {
        private CriteriaDAL criteriaContext = new CriteriaDAL();

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            int weightage = Convert.ToInt32(value);
            Criteria criteria = (Criteria)validationContext.ObjectInstance;

            string criteriaName = criteria.CriteriaName;
            int competitionID = criteria.CompetitionID;

            if (criteriaContext.IsCriteriaWeightage100(weightage, competitionID))
                return ValidationResult.Success;
            
            else
                return new ValidationResult("Total weightage cannot be more than 100!");
        }
    }
}
