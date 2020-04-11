using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Authorization;
using SecretMagic.Model;
using SecretMagic.Repository;

namespace SecretMagic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Protected("User Management")]
    public class UserRoleMappingController : ControllerBase
    {
        private readonly ILogger<UserRoleMappingController> logger;
        private readonly IUrmRepository urmrRepository;

        public UserRoleMappingController(ILogger<UserRoleMappingController> logger, IUrmRepository repository)
        {
            this.logger = logger;
            this.urmrRepository = repository;
        }

        [HttpGet("All")]
        public IEnumerable<UserRoleMapping> All()
        {
            return urmrRepository.Read();
        }

        [HttpPost("New")]
        public async Task<IActionResult> Add(UserRoleMapping urm)
        {
            await urmrRepository.Create(urm);
            return Ok();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, UserRoleMapping urm)
        {
            if(urm == null)
            {
                return BadRequest();
            }

            urm.Id = new Guid(id);
            var result = await urmrRepository.Update(urm);
            if(result == 1)
            {
                return Ok();
            }
            return NoContent();
        }

        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Delete(string id, UserRoleMapping urm)
        {
            urm.Id = new Guid(id);
            var result = await urmrRepository.Delete(urm);
            if(result == 1)
            {
                return Ok();
            }
            return NoContent();
        }
    }
}