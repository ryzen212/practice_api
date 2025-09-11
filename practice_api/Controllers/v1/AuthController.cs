using Asp.Versioning;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NuGet.Common;
using practice_api.Contracts;
using practice_api.Data;
using practice_api.Models.Auth;


namespace practice_api.Controllers.v1
{

    [ApiController]

    [Route("api/v{version:apiVersion}/auth")]
    [ApiVersion("1.0")]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAuthServices _authService;
     

        public AuthController(AppDbContext context , IAuthServices authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login(AuthLoginRequest request)
        {
            try
            {
                var result = await _authService.Login(request);

                if (result.Error) {
                    return Unauthorized(result);
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = true,
                    header = "Error",
                    message = "An unexpected error occurred." // hide ex.Message in production
                });
            }

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthRegister request)
        {
            try
            {
                var result = await _authService.Register(request);

                if (result.Error)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = true,
                    header = "Error",
                    message = "An unexpected error occurred." // hide ex.Message in production
                });
            }
        }


        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            try {
                var result = await _authService.Refresh(refreshToken);
                return Ok (result);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = true,
                    header = "Error",
                    message = "An unexpected error occurred." // hide ex.Message in production
                });
            }
        }
        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized();
                }
              var result =  await _authService.GetUser(userId);

                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = true,
                    header = "Error",
                    message = "An unexpected error occurred." // hide ex.Message in production
                });
            }
            // 👇 The token is automatically validated by middleware
            
                

            // return what you want
     
        }

        [Authorize]
        [HttpGet("test")]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are Authenticated");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("test-role")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are Authenticated");
        }

    }
}
