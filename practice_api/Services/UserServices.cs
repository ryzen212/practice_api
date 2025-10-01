using Azure.Core;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using practice_api.Contracts;
using practice_api.Data;
using practice_api.Models.Auth;
using practice_api.Models.Shared;
using practice_api.Models.Users;
using practice_api.Validations;
using System.Linq.Dynamic.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace practice_api.Services
{   
   
    public class UserServices : IUserServices
    {
        private readonly FileService _fileService;
        private readonly IUserRepository _userRepo;
        private readonly AppDbContext _context;
        private readonly IValidationService _validationService;
       

        public UserServices(IUserRepository userRepo,  AppDbContext context, IValidationService validationService, FileService fileService)
        {
            _userRepo = userRepo;
            _context = context;
            _validationService = validationService;
            _fileService = fileService;
        }
       public async Task<ServiceResult> Create(UserCreateDto request)
        {
            var errors =await _validationService.ValidateAsync(request);

            if (errors != null) {
                return ServiceResult.FailWithErrors(errors);
            }
   
            var avatar = _fileService.PrepareSaveFile(request.Avatar, "uploads");
          
            var user = new AppIdentityUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Avatar = avatar.FilePath
            };

            await _fileService.UploadAsync(request.Avatar, avatar.FileName, "uploads");

            await _userRepo.CreateAsync(user,request.Password);
            await _userRepo.AddToRoleAsync(user, request.Role);


            return ServiceResult.Success("Success","User created successfully.");

        }

        public async Task<ServiceResult> Update(string id, UserUpdateDto request)
        {
           Console.WriteLine(request.Id);
            var errors = await _validationService.ValidateAsync(request);

            if (errors != null)
            {
                return ServiceResult.FailWithErrors(errors);
            }

            var avatar = _fileService.PrepareSaveFile(request.Avatar, "uploads");

            var user = await _userRepo.FindByIdAsync(id);
            var oldImg = user.Avatar;

            user.PhoneNumber = request.PhoneNumber;
            user.UserName = request.UserName;
            user.Email = request.Email;
            user.Avatar = avatar.FilePath;


        

            await _userRepo.UpdateAsync(user);
            await _userRepo.RemoveFromRoleAsync(user, request.Role);
            await _userRepo.AddToRoleAsync(user, request.Role);

            await _fileService.UploadAsync(request.Avatar, avatar.FileName, "uploads");
            _fileService.DeleteFile(oldImg);

            return ServiceResult.Success("Success", "User Updated successfully.");

        }

        public async Task<TableResponse<UserDto>> Table(TableRequest request)
        {
            var query =  (from users in _context.Users
                               join userRoles in _context.UserRoles on users.Id equals userRoles.UserId into ur
                               from userRoleJoin in ur.DefaultIfEmpty()
                               join roles in _context.Roles on userRoleJoin.RoleId equals roles.Id into rr
                               from roleJoin in rr.DefaultIfEmpty()
                               select new UserDto
                               {
                                 Id = users.Id,
                                 UserName = users.Email,
                                 Role = roleJoin.Name,
                                 Avatar = users.Avatar,
                                 Email = users.Email,
                                 PhoneNumber = users.PhoneNumber,

                               });

            if (!string.IsNullOrEmpty(request.search))
            {
                query = query.Where(u =>
                    u.UserName.Contains(request.search) ||
                    u.Email.Contains(request.search) ||
                    u.Role.Contains(request.search));
            }
            if (!string.IsNullOrEmpty(request.sortField))
            {
                var direction = request.sortOrder == 1 ? "asc" : "desc";
                query = query.OrderBy($"{request.sortField} {direction}");
            }
            else
            {
                query = query.OrderBy(u => u.UserName); // default sort
            }
         
            int totalRecords = await query.CountAsync();
            var data = await query.Skip(request.first).Take(request.rows).ToListAsync();
       
            return new TableResponse<UserDto>
            {
                data = data,
                totalRecords = totalRecords
            };

        }
       
    }
}
