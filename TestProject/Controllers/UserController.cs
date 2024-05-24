using Microsoft.AspNetCore.Mvc;
using TestProjectServicePart.Services;
using TestProjectServicePart.ModelDtos;
using TestProjectDbPart.Models;
using Microsoft.AspNetCore.Authorization;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("AddUser")]
        public async Task<bool> AddUser(UserModelDto user)
        {
            return await _userService.AddUser(user);
        }

        [HttpPut("{Id:int}")]
        public async Task<bool> EditUser(UserModel user)
        {
            if (user == null)
            {
                return false;
            }
            return await _userService.EditUser(user);
        }

        [HttpDelete("{Id:int}")]
        public async Task<bool> RemovUser(int Id)
        {
            return await _userService.RemoveUser(Id);
        }

        [HttpGet("{Id:int}")]
        public async Task<IResult> GetUser(int Id)
        {
            var user = await _userService.GetUser(Id);
            if (user == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(user);
        }

        [HttpGet("GetUsers")]
        public IResult GetUsers()
        {
            var user = _userService.GetUsers();
            if (user == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(user);
        }
    }
}
