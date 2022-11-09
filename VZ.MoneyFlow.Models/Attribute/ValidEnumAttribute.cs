using System;
using System.ComponentModel.DataAnnotations;


namespace VZ.MoneyFlow.Models.Attribute
{
    public class ValidEnumAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
        {
            var type = value.GetType();

            if (value == null || !(type.IsEnum && Enum.IsDefined(type, value)))
                return new ValidationResult(ErrorMessage ?? $"{value} is not a valid value for type {type.Name}");
                        
            return ValidationResult.Success;
        }
    }
}
