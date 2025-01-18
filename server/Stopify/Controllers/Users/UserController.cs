using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stopify.Attribute.Auth;
using Stopify.Repositories.Users;
using Stopify.Requests.Users;
using Stopify.Services.Users;
using UserEntity = Stopify.Entities.Users.User;

namespace Stopify.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UserController(UserRepository userRepository, UserService userService) : ControllerBase
{
    [Authorize]
    [AuthorizeUser]
    [HttpGet("{userId:int}", Name = "GetUser")]
    public ActionResult<UserEntity> Get(int userId)
    {
        var user = userRepository.GetById(userId);

        return user == null ? NotFound() : Ok(user);
    }

    [HttpGet( Name = "GetAllUsers")]
    public ActionResult<IEnumerable<UserEntity>> GetAll()
    {
        var users = userRepository.GetAll();

        return Ok(users);
    }

    [AllowAnonymous]
    [HttpPost( Name = "CreateUser")]
    public ActionResult Create([FromBody] CreateUserRequest request)
    {
        userService.AddUser(request.Username, request.Email, request.Password);

        return Created();
    }

    [Authorize]
    [AuthorizeUser]
    [HttpPut("{userId:int}")]
    public ActionResult Update(int userId, [FromBody] UpdateUserRequest request)
    {
        if (!userService.UserExists(userId))
        {
            return NotFound(new { message = $"User with ID {userId} not found." });
        }

        var updatedUser = userService.UpdateUser(userId, request.Username, request.Email);

        return Ok(updatedUser);
    }

    [Authorize]
    [AuthorizeUser]
    [HttpDelete("{userId:int}")]
    public ActionResult Delete(int userId)
    {
        if (!userService.UserExists(userId))
        {
            return NotFound(new { message = $"User with ID {userId} not found." });
        }

        userRepository.Delete(userId);

        return Ok();
    }
}