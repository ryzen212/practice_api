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
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/auth")]

    public class AuthController : Controller
    {

        private readonly IAuthServices _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthServices authService, ILogger<AuthController> logger)
        {
      
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {

                var result = await _authService.Login(request);
                if (result.Error)
                {
                    return result.Errors.Any() ?  UnprocessableEntity(result) : Unauthorized(result);
                }
           
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " - Login Error");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = true,
                    header = "Error",
                    message = "An unexpected error occurred." // hide ex.Message in production
                });
            }

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
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
                _logger.LogError(ex, "- Register Error");
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
                _logger.LogError(ex, "- Refresh token Error");
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
                _logger.LogError(ex, "- Authcontroller Getuser");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = true,
                    header = "Error",
                    message = "An unexpected error occurred." // hide ex.Message in production
                });
            }
           
     
        }


    }
}
