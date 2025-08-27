
using System.ComponentModel.DataAnnotations;

namespace Lpul_Inventory.Validations
{
    public class BaseValidation
    {
        public Dictionary<string, string[]> Validate<M>(M model) where M : class
        {

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);

            if (!isValid)
            {
                var errors = results
                     .SelectMany(r => r.MemberNames.Select(field => new { field, r.ErrorMessage }))
                     .GroupBy(x => x.field)
                     .ToDictionary(
                         g => g.Key,
                         g => g.Select(x => x.ErrorMessage).ToArray()
                     );

                return errors;
            }
            else
            {
                return new Dictionary<string, string[]>();
            }
        }
    }
}
