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
using SecretMagic.Model;
using SecretMagic.Repository;

namespace SecretMagic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Protected("Role Permission Management")]
    public class PermissionController : ControllerBase
    {
        private readonly ILogger<PermissionController> logger;
        private readonly IPermissionRepository permissionRepository;

        public PermissionController(ILogger<PermissionController> logger, IPermissionRepository permissionRepository)
        {
            this.logger = logger;
            this.permissionRepository = permissionRepository;
        }

        [HttpGet("All")]
        public IEnumerable<Permission> All()
        {
            return permissionRepository.Read();
        }

        [HttpPost("New")]
        public async Task<IActionResult> Add(Permission permission)
        {
            await permissionRepository.Create(permission);
            return Ok();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, Permission permission)
        {
            permission.Id = new Guid(id);
            var result = await permissionRepository.Update(permission);
            if(result == 1)
            {
                return Ok();
            }
            return NoContent();
        }

        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Delete(string id, Permission permission)
        {
            permission.Id = new Guid(id);
            var result = await permissionRepository.Delete(permission);
            if(result == 1)
            {
                return Ok();
            }
            return NoContent();
        }
    }
}