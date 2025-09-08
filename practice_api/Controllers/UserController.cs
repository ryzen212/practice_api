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
        private readonly IValidationService _validationService;

        public UserController(IUserServices userService, IValidationService validationService) {
            _userService = userService;
            _validationService = validationService;
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
        public async Task<IActionResult> Create([FromBody] UserCreateDto request)
        {
            try {
                var result = await _userService.Create(request);
    
                if (result.Errors != null) { 
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
                    message = e.Message // hide ex.Message in production
                });
            }
  
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDto request) { 
            try
            {
           
                request.Id = id;
                var result = await _userService.Update(id,request);

                if (result.Errors != null)
                {
                    return UnprocessableEntity(result);
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = true,
                    status = "Error",
                    message = e.Message // hide ex.Message in production
                });
            }

        }


    }
}
