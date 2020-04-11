using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Authorization;
using SecretMagic.API.Commom;
using SecretMagic.API.Services;
using SecretMagic.Model;
using SecretMagic.Repository;

namespace SecretMagic.API.Controllers
{
    /// <summary>
    /// API for SuperAdmin data testing
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> logger;
        private readonly IRoleService roleRoleService;

        public RoleController(ILogger<RoleController> logger, IRoleService roleRoleService)
        {
            this.logger = logger;
            this.roleRoleService = roleRoleService;
        }

        [HttpGet("AllRoles")]
        [Protected("User Management,Role Permission Management")]
        public ActionResult<IEnumerable<RoleInfo>> GetAllRoles()
        {
            try
            {
                return Ok(roleRoleService.GetAllRoles());
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

        [HttpGet("AllResources")]
        [Protected("Role Permission Management")]
        public ActionResult<IEnumerable<Resource>> GetAllResources()
        {
            try
            {
                return Ok(roleRoleService.GetAllResources());
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

        [HttpPut("Update/{id}")]
        [Protected("Role Permission Management")]
        public IActionResult Update(RoleInfo roleInfo)
        {
            try
            {
                roleRoleService.UpdateRole(roleInfo);
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

            return Ok();
        }

        [HttpPost("NewRole")]
        [Protected("Role Permission Management")]
        public async Task<IActionResult> Add(Role role)
        {
            try
            {
                var result = await roleRoleService.AddRole(role);
                if (result != null)
                {
                    return Ok();
                }
                return NoContent();
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

        [HttpDelete("RemoveRole/{id}")]
        [Protected("Role Permission Management")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await roleRoleService.DeleteRole(new Guid(id));
                if (result == 1)
                {
                    return Ok();
                }
                return NoContent();
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

        [HttpPost("NewPermission")]
        [Protected("Role Permission Management")]
        public async Task<IActionResult> AddPermission(Permission permission)
        {
            try
            {
                var result = await roleRoleService.AddPermission(permission);
                if (result != null)
                {
                    return Ok();
                }
                return NoContent();
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

        [HttpDelete("RemovePermission")]
        [Protected("Role Permission Management")]
        public async Task<IActionResult> DeletePermission(Permission permission)
        {
            try
            {
                var result = await roleRoleService.DeletePermission(permission);
                if (result == 1)
                {
                    return Ok();
                }
                return NoContent();
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