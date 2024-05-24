using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestProjectServicePart.ModelDtos;

namespace TestProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController: ControllerBase
    {
        private readonly TestProjectServicePart.Services.AuthenticationService _service;

        public AuthenticationController(TestProjectServicePart.Services.AuthenticationService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("Registration")]
        public IResult RegisterUser(AuthenticationRegisterDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var result = _service.Registration(user);
            return Results.Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("LogIn")]
        public IResult LogIn(AuthenticationLoginDto logIn)
        {
            if (logIn == null)
            {
                throw new ArgumentNullException(nameof(logIn));
            }

            var result = _service.LogIn(logIn);
            return Results.Ok(result.Values);
        }

        [HttpGet("RefreshToken")]
        public IResult Refresh()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Results.Ok(_service.RefreshToken(token));
        }
    }
}
