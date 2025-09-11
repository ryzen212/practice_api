using FluentValidation;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using practice_api.Contracts;
using practice_api.Models.Users;
using System.Threading.Tasks;

namespace practice_api.Validations.User
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        public UserUpdateValidator(IRoleRepository roleRepository, IUserRepository userRepository) {
         _roleRepository = roleRepository;
         _userRepository = userRepository;


            RuleFor(x=>x.UserName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Username is Required")
            .MustAsync(UniqueUsername).WithMessage("Username already taken");

            RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is Required")
            .MustAsync(UniqueEmail).WithMessage("Email already taken");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phonenumber is Required");

            RuleFor(x => x.Role)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Role is Required")
            .MustAsync( RoleExistsAsync).WithMessage("Role does not exist"); 

        }

        private async Task<bool> RoleExistsAsync(string role, CancellationToken cancellationToken)
        {
         
            return await _roleRepository.RoleExistsAsync(role);

        }

        private async Task<bool> UniqueEmail(UserUpdateDto dto, string email, CancellationToken cancellationToken)
        {

            return !await _userRepository.EmailExistsAsync(email, dto.Id);

        }
        private async Task<bool> UniqueUsername(UserUpdateDto dto, string username, CancellationToken cancellationToken)
        {

            return !await _userRepository.UserNameExistAsync(username, dto.Id);

        }
    }
}
