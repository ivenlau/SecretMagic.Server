using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Services;
using SecretMagic.Model;

namespace SecretMagic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> logger;
        private readonly ITokenService tokenService;
        private readonly DataContext context;

        public AdminController(ILogger<AdminController> logger, ITokenService tokenService, DataContext context)
        {
            this.logger = logger;
            this.tokenService = tokenService;
            this.context = context;
        }

        [HttpGet("InitialDatabase/{secret}")]
        public IActionResult InitialDatabase(string secret)
        {
            if (secret == "www.secretmagic.tech")
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var userId = Guid.NewGuid();
                var roleId = Guid.NewGuid();
                var resourceId = Guid.NewGuid();

                context.Users.Add(new User { Id = userId, Name = "Admin", Password="????n????:b??????]Z?????:?l?" });
                context.Roles.Add(new Role { Id = roleId, Name = "SuperAdmin" });
                context.URM.Add(new UserRoleMapping { UserId = userId, RoleId = roleId });
                context.Resources.Add(new Resource{ Name = "User Management" });
                context.Resources.Add(new Resource{ Id = resourceId, Name = "Role Permission Management" });
                context.Resources.Add(new Resource{ Name = "System Management" });
                context.Resources.Add(new Resource{ Name = "Blog Category" });
                context.Resources.Add(new Resource{ Name = "Blog Post" });
                context.Resources.Add(new Resource{ Name = "Blog Management" });
                context.Permissions.Add(new Permission{ RoleId = roleId, ResourceId = resourceId});
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("Invalid Secret!");
            }
        }
    }
}