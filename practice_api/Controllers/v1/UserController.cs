using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_api.Contracts;
using practice_api.Models.Shared;
using practice_api.Models.Users;


namespace practice_api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/users")]
 
    public class UserController : Controller
    {
        private readonly IUserServices _userService;


        public UserController(IUserServices userService) {
            _userService = userService;
           
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] TableRequest request)
        {
               var table =await _userService.Table(request);
            return Ok(table);
        }


   
        [HttpPost]
        public async Task<IActionResult> Create( UserCreateDto request)
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
        public async Task<IActionResult> Update(string id, [FromForm] UserUpdateDto request) { 
            try
            {
           
             
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
