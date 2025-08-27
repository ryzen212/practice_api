using Lpul_Inventory.Validations;

using practice_api.Models.Shared;
using practice_api.Models.Users;

namespace practice_api.Validations
{
    public class UserValidation : BaseValidation
    {
        public  ValidationResult Create(UserCreateDto user)
        {

            var errors = this.Validate(user);
       

            var result = new ValidationResult { 
            Errors= errors
            };

            return result;

        }
    }
}
