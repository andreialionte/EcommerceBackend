using Ecommerce2.Data;
using Ecommerce2.Dtos;
using Ecommerce2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce2.Controllers
{
    public class UsersController : ControllerBase
    {

        private readonly DataContextEf _ef;
        private readonly IConfiguration _config;

        public UsersController(IConfiguration configuration)
        {
            _config = configuration;
            _ef = new DataContextEf(configuration);
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser()
        {
            IEnumerable<User> users = _ef.users.ToList();
            return Ok(users);
        }
        [HttpPost("AddUser")]
        public IActionResult AddUser(UserDto userDto, int Id)
        {
            User userDb = new User()
            {
                Email = userDto.Email,
                Username = userDto.Username,
                RegistrationDate = DateTime.Now,
                UserId = Id
            };
            _ef.users.Add(userDb);
            _ef.SaveChanges();
            return Ok(userDb);
        }

    }
}
