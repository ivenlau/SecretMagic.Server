using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SecretMagic.Model;
using SecretMagic.Repository;

namespace SecretMagic.API.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ILogger<AuthorizationService> logger;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUrmRepository urmRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IResourceRepository resourceRepository;
        private readonly ITokenService tokenService;

        public AuthorizationService(ILogger<AuthorizationService> logger,
            IUserRepository repository,
            IRoleRepository roleRepository,
            ITokenService tokenService,
            IUrmRepository urmRepository,
            IPermissionRepository permissionRepository,
            IResourceRepository resourceRepository
            )
        {
            this.logger = logger;
            this.userRepository = repository;
            this.roleRepository = roleRepository;
            this.urmRepository = urmRepository;
            this.tokenService = tokenService;
            this.permissionRepository = permissionRepository;
            this.resourceRepository = resourceRepository;
        }

        public async Task<bool> IsAuthorized(string userId, string[] resources)
        {
            var isAuthorized = false;
            try
            {
                var id = new Guid(userId);
                var urm = await urmRepository.GetUrmByUserId(id);
                if (urm != null)
                {
                    var role = roleRepository.GetById(urm.RoleId);
                    if (role != null)
                    {
                        IQueryable<Permission> permissions = permissionRepository.GetAllByRoleId(role.Id);
                        if (permissions != null)
                        {
                            permissions.ToList().ForEach(p =>
                            {
                                var resource = resourceRepository.GetById(p.ResourceId);
                                if (resources.Any(r => r.Trim() == resource.Name.Trim()))
                                {
                                    isAuthorized = true;
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(403, e, e.Message);
            }

            return isAuthorized;
        }
    }
}