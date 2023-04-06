using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.Helpers
{
    public class ValidationHelper
    {
        /// <summary>
        /// Uses ValidationContext to validate a model and throws ArgumentException in case of any validation errors
        /// </summary>
        /// <param name="obj">Model object to validate</param>
        /// /// <exception cref="ArgumentException">When one or more validation errors found</exception>
        internal static void ModelValidation(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
