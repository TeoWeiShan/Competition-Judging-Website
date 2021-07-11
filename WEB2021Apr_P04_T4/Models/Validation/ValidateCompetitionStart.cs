using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Models.Validation
{
    public class ValidateCompetitionStart : ValidationAttribute
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime startDate = Convert.ToDateTime(value);
                Competition competition = (Competition)validationContext.ObjectInstance;
                int competitionID = competition.CompetitionID;
                if (!competitionContext.IsModifiable(competitionID))
                {
                    if (competition.StartDate > DateTime.Today.AddYears(5) )
                    {
                        return new ValidationResult
                           ("Start Date cannot be 5 years later than Today's Date!");
                    }
                    else
                    {
                        if (startDate < DateTime.Today)
                        {
                            // validation failed
                            return new ValidationResult
                            ("Start Date cannot be earlier than Today's Date!");

                        }
                        else
                        {
                            if (competition.EndDate == null && competition.ResultReleasedDate == null)
                            {
                                return new ValidationResult
                                ("Rest of the dates cannot be null!");
                            }
                            else if (competition.ResultReleasedDate == null)
                            {
                                return new ValidationResult
                               ("Result Release Date cannot be null!");
                            }
                            else if (competition.EndDate == null)
                            {
                                return new ValidationResult
                               ("End Date cannot be null!");
                            }
                            else
                            {
                                // validation passed
                                return ValidationResult.Success;
                            }
                        }
                    }
                        
                    
                   
                }
                else { return ValidationResult.Success; }
            }
            else { return ValidationResult.Success; }
        }

    }
}
