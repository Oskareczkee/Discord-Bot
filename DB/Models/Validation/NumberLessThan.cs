using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DB.Models.Validation
{
    public class NumberLessThanAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _comparisonProperty;

        public NumberLessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            int currentValue = (int)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty) ?? throw new ArgumentException("Property with this name has not been found");
            var comparisonValue = (int)property.GetValue(validationContext.ObjectInstance);

            if (currentValue > comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-error", error);
        }
    }
}
