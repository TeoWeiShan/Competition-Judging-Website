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
            string criteriaName = Convert.ToString(value);
            Criteria criteria = (Criteria)validationContext.ObjectInstance;

            int criteriaID = criteria.CriteriaID;
            int competitionID = criteria.CompetitionID;
            int Weightage = criteria.Weightage;

            if (Weightage <= 100)
                return ValidationResult.Success;
            
            else
                return new ValidationResult("Total weightage cannot be more than 100!");
        }
    }
}
