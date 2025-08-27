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
  
        
        public UserController(IUserServices userServices) {
            _userService = userServices;
      
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
