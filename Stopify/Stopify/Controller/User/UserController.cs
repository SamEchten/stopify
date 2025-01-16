using Microsoft.AspNetCore.Mvc;
using Stopify.Repository.User;
using UserEntity =  Stopify.Entity.User.User;

namespace Stopify.Controller.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        public ActionResult<UserEntity> Get(int id)
        {
            var user = _userRepository.GetById(id);
            
            return user == null ? NotFound() : user;
        }
    }
}