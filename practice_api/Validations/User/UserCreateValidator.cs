using FluentValidation;
using Microsoft.EntityFrameworkCore;
using practice_api.Contracts;
using practice_api.Models.Users;
using System.Threading.Tasks;

namespace practice_api.Validations.User
{
    public class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        public UserCreateValidator(IRoleRepository roleRepository, IUserRepository userRepository) {
         _roleRepository = roleRepository;
         _userRepository = userRepository;


            RuleFor(x=>x.UserName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Username is Requiredsd")
            .MustAsync(UniqueUsername).WithMessage("Username already taken");

            RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is Required")
            .MustAsync(UniqueEmail).WithMessage("Email already taken");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phonenumber is Required");
            
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is Required");

            RuleFor(x => x.Role)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Role is Required")
            .MustAsync( RoleExistsAsync).WithMessage("Role does not exist");

            RuleFor(x => x.Avatar)
            .Cascade(CascadeMode.Stop)
            .Must(file =>
            {
            if (file == null) return true; // allow null
            var ext = Path.GetExtension(file.FileName).ToLower();
            return new[] { ".jpg", ".jpeg", ".png" }.Contains(ext);
            })
            .WithMessage("Only .jpg, .jpeg, and .png files are allowed.")

            .Must(file => file == null || file.Length <= 2 * 1024 * 1024) // 2 MB
            .WithMessage("Avatar must not exceed 2MB.");

        }

        private async Task<bool> RoleExistsAsync(string role, CancellationToken cancellationToken)
        {
         
            return await _roleRepository.RoleExistsAsync(role);

        }

        private async Task<bool> UniqueEmail(string email, CancellationToken cancellationToken)
        {

            return !await _userRepository.EmailExistsAsync(email);

        }
        private async Task<bool> UniqueUsername(string username, CancellationToken cancellationToken)
        {

            return !await _userRepository.UserNameExistAsync(username);

        }
    }
}
