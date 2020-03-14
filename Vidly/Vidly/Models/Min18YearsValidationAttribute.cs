using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.OAuth;

namespace Vidly.Models
{
    public class Min18YearsValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = validationContext.ObjectInstance as Customer;
            if (customer.MembershipTypeId == 1)
                return ValidationResult.Success;
            if (customer?.BirthDate == null)
                return new ValidationResult("Birthdate is required.");
            var age = DateTime.Today.Year - customer.BirthDate.Value.Year;
            return age < 18
                ? new ValidationResult("Customer should be at least 18 years of age to become a member.")
                : ValidationResult.Success;
        }
    }
}