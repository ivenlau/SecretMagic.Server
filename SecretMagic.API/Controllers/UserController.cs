using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Authorization;
using SecretMagic.API.Commom;
using SecretMagic.API.Services;
using SecretMagic.Model;

namespace SecretMagic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Protected("User Management")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        [HttpGet("AllUsers")]
        public ActionResult<IEnumerable<UserInfo>> AllUsers()
        {
            try
            {
                return Ok(userService.GetAllUsers());
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

        [HttpPost("NewUser")]
        public async Task<IActionResult> NewUser(UserInfo user)
        {
            try
            {
                await userService.AddUser(user);
                return Ok();
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

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserInfo user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            try
            {
                await userService.UpdateUser(user);
                return Ok();
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

        [HttpDelete("RemoveUser/{id}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            try
            {
                await userService.DeleteUser(id);
                return Ok();
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