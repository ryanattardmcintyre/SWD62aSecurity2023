using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.Validators
{
    public class YearCustomValidator: ValidationAttribute
    {
        public string GetErrorMessage() =>
            $"Books must have a publish year no later than {DateTime.Now.Year}.";

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var book = (BookViewModel)validationContext.ObjectInstance;
          

            if (book.Year  > DateTime.Now.Year)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
