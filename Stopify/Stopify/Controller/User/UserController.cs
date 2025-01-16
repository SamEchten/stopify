using Microsoft.AspNetCore.Mvc;
using Stopify.Repository.User;
using Stopify.Service.User;
using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Controller.User;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserRepository userRepository, UserService userService) : ControllerBase
{
    [HttpGet("{id:int}", Name = "GetUser")]
    public ActionResult<UserEntity> Get(int id)
    {
        var user = userRepository.GetById(id);
        
        return user == null ? NotFound() : Ok(user);
    }

    [HttpGet( Name = "GetAllUsers")]
    public ActionResult<IEnumerable<UserEntity>> GetAll()
    {
        var users = userRepository.GetAll();

        return Ok(users);
    }

    [HttpPost( Name = "CreateUser")]
    public ActionResult Create(UserEntity user)
    {
        userService.AddUser(user);
            
        return Created();
    }
    
    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] UserEntity user)
    {
        if (!userService.UserExists((id)))
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }
        
        var updatedUser = userService.UpdateUser(id, user);

        return Ok(updatedUser);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        if (!userService.UserExists((id)))
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }
        
        userRepository.Delete(id);

        return Ok();
    }
}