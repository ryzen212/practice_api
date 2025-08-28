using FluentValidation;
using Microsoft.EntityFrameworkCore;
using practice_api.Models.Users;

namespace practice_api.Validations.User
{
    public class UserCreateValidator : AbstractValidator<UserCreateDto>
    {

        public UserCreateValidator() { 
        RuleFor(x=>x.UserName).NotEmpty().WithName("userName").WithMessage("Username is Required");
        RuleFor(x => x.Email).NotEmpty().WithName("emai").WithMessage("Email is Required");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithName("username").WithMessage("Phonenumber is Required");
        RuleFor(x => x.Password).NotEmpty().WithName("username").WithMessage("Password is Required");
        RuleFor(x => x.Role).NotEmpty().WithName("username").WithMessage("Role is Required");
        }

      

    }
}
