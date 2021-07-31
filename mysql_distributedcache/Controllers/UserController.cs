
namespace mysql_distributedcache.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using mysql_distributedcache.DTO;
    using mysql_distributedcache.Models;
    using mysql_distributedcache.Service;
    using mysql_distributedcache.TokenHandler;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        private readonly ITokenManager tokenManager;

        public UserController(
            UserService userService,
            ITokenManager tokenManager)
        {
            this.userService = userService;

            this.tokenManager = tokenManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get()
        {
            IList<User> users = await userService.FetchAll();

            return Ok(users);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post([FromBody]User user)
        {
            bool inserted = await userService.Insert(user);

            if(inserted)
            {
                return Created($"~api/user/", user);
            }

            return BadRequest();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete([FromBody] string email)
        {
            User user = await userService.GetUser(email);

            if (user == null)
            {
                return BadRequest();
            }

            await userService.Delete(user);

            await tokenManager.DeactivateAsync(user.Id.ToString());

            return Accepted();
        }

        [HttpPost]
        [Route("deactivatesubscription/{email}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeactivateSubscription(string email)
        {
            User user = await userService.GetUser(email);

            if (user == null)
            {
                return BadRequest();
            }

            await userService.DeactivateSubscription(user);

            await tokenManager.DeactivateCurrentAsync();

            return Accepted();
        }
    }
}
