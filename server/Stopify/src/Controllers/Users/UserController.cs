using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stopify.Attribute.Auth;
using Stopify.Entities.Users.DTO;
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
    [HttpGet("me", Name = "GetMe")]
    public ActionResult<UserProfileDTO> GetMe()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = userRepository.GetById(userId);
        if (user == null) return NotFound();

        var dto = new UserProfileDTO(
            user.Id,
            user.Username,
            user.Email,
            user.Roles.Select(r => r.Name).ToList(),
            user.CreatedAt
        );

        return Ok(dto);
    }

    [Authorize]
    [HttpPost("change-password", Name = "ChangePassword")]
    public ActionResult ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var success = userService.ChangePassword(userId, request.CurrentPassword, request.NewPassword);
        if (!success) return BadRequest(new { message = "Current password is incorrect." });
        return Ok();
    }

    [Authorize]
    [AuthorizeUser]
    [HttpGet("{userId:int}", Name = "GetUser")]
    public ActionResult<UserEntity> Get(int userId)
    {
        var user = userRepository.GetById(userId);

        return user == null ? NotFound() : Ok(user);
    }

    [AllowAnonymous]
    [HttpGet( Name = "GetAllUsers")]
    public ActionResult<ICollection<GetAllUsersDTO>> GetAll()
    {
        var users = userRepository.GetAll();

        return Ok(users);
    }

    [AllowAnonymous]
    [HttpPost( Name = "CreateUser")]
    public ActionResult<UserEntity> Create([FromBody] CreateUserRequest request)
    {
        var user = userService.CreateUser(request.Username, request.Email, request.Password);

        return user;
    }

    [Authorize]
    [AuthorizeUser]
    [HttpPut(Name = "UpdateUser")]
    public ActionResult Update([FromBody] UpdateUserRequest request)
    {
        if (!userService.UserExists(request.UserId))
        {
            return NotFound(new { message = $"User with ID {request.UserId} not found." });
        }

        var updatedUser = userService.UpdateUser(request.UserId, request.Username, request.Email);

        return Ok(updatedUser);
    }

    [Authorize]
    [AuthorizeUser]
    [HttpDelete("{userId:int}", Name = "DeleteUser")]
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