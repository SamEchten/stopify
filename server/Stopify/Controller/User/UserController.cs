using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stopify.Attribute.Auth;
using Stopify.Repository.User;
using Stopify.Service.User;
using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Controller.User;

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
    public ActionResult Create(UserEntity user)
    {
        userService.AddUser(user);
            
        return Created();
    }
    
    [Authorize]
    [AuthorizeUser]
    [HttpPut("{userId:int}")]
    public ActionResult Update(int userId, [FromBody] UserEntity user)
    {
        if (!userService.UserExists((userId)))
        {
            return NotFound(new { message = $"User with ID {userId} not found." });
        }
        
        var updatedUser = userService.UpdateUser(userId, user);

        return Ok(updatedUser);
    }

    [Authorize]
    [AuthorizeUser]
    [HttpDelete("{userId:int}")]
    public ActionResult Delete(int userId)
    {
        if (!userService.UserExists((userId)))
        {
            return NotFound(new { message = $"User with ID {userId} not found." });
        }
        
        userRepository.Delete(userId);

        return Ok();
    }
}