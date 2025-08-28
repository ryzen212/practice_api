using FluentValidation;
using Humanizer;
using Lpul_Inventory.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using practice_api.Contracts;
using practice_api.Models.Shared;
using practice_api.Models.Users;
using practice_api.Services;
using practice_api.Validations;

namespace practice_api.Controllers
{

    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserServices _userService;
        private readonly IValidator<UserCreateDto> _userCreateValidator;

        public UserController(IUserServices userService, IValidator<UserCreateDto> userCreateValidator) {
            _userService = userService;
            _userCreateValidator = userCreateValidator;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]  TableRequest request)
        {
               var table =await _userService.Table(request);
            return Ok(table);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto request)
        {
            try {

                var validationResults = _userCreateValidator.Validate(request);
                if (!validationResults.IsValid)
                {
                var errors = validationResults.Errors
                .GroupBy(e => char.ToLowerInvariant(e.PropertyName[0]) + e.PropertyName.Substring(1)) // camelCase
                .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
    );
                    return BadRequest(new
                    {
                        error = true,
                        status = "Adding user failed",
                        errors = errors,
                        messsage = "Please try again."

                    });
                }

                var result = await _userService.Create(request);
    
                if (result.Errors != null)
                {
                    return UnprocessableEntity(result);
                }
                return Ok(result);
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = true,
                    status = "Error",
                    message = "An unexpected error occurred." // hide ex.Message in production
                });
            }
  
        }
    

    }
}
