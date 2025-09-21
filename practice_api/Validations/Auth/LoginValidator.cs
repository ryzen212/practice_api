using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

using LoginRequest = practice_api.Models.Auth.LoginRequest;
namespace practice_api.Validations.User
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {

            RuleFor(x => x.UserName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Username is Required");

            RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password is Required");

        }

      
    }
}
