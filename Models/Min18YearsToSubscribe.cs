using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Models
{
    public class Min18YearsToSubscribe : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime birthdate = (DateTime)value;
            birthdate = birthdate.AddYears(18);

            if (birthdate < DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("You must be at least 18 years old to subscribe");
            }
        }
    }
}
