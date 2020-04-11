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
    /// <summary>
    /// API for SuperAdmin data testing
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles="SuperAdmin")]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> logger;
        private readonly IResourceRepository resourceRepository;

        public ResourceController(ILogger<ResourceController> logger, IResourceRepository repository)
        {
            this.logger = logger;
            this.resourceRepository = repository;
        }

        [HttpGet]
        public IEnumerable<Resource> All()
        {
            return resourceRepository.Read();
        }

        [HttpPost("New")]
        public async Task<IActionResult> Add(Resource resource)
        {
            await resourceRepository.Create(resource);
            return Ok();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, Resource resource)
        {
            resource.Id = new Guid(id);
            var result = await resourceRepository.Update(resource);
            if(result == 1)
            {
                return Ok();
            }
            return NoContent();
        }

        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Delete(string id, Resource resource)
        {
            resource.Id = new Guid(id);
            var result = await resourceRepository.Delete(resource);
            if(result == 1)
            {
                return Ok();
            }
            return NoContent();
        }
    }
}