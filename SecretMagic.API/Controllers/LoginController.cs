using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Commom;
using SecretMagic.API.Services;
using SecretMagic.Model;

namespace SecretMagic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly ILoginService loginService;

        public LoginController(ILogger<UserController> logger, ILoginService loginService)
        {
            this.logger = logger;
            this.loginService = loginService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm]string userName, [FromForm]string password)
        {
            try
            {
                var result = await loginService.Login(userName, password);
                return Ok(result);
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }
    }
}