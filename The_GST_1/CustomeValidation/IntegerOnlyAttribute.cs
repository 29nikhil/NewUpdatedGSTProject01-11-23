using System.ComponentModel.DataAnnotations;

namespace The_GST_1.CustomeValidation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IntegerOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // null values are considered valid
            }

            if (value is int)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("The field must contain only integer values.");
        }
    }

}
