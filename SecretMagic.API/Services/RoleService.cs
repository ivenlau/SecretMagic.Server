using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Commom;
using SecretMagic.Model;
using SecretMagic.Repository;

namespace SecretMagic.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> logger;
        private readonly IRoleRepository roleRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IResourceRepository resourceRepository;

        public RoleService(ILogger<RoleService> logger,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IResourceRepository resourceRepository
            )
        {
            this.logger = logger;
            this.roleRepository = roleRepository;
            this.permissionRepository = permissionRepository;
            this.resourceRepository = resourceRepository;
        }

        public IEnumerable<RoleInfo> GetAllRoles()
        {
            var result = new List<RoleInfo>();
            var allRoles = roleRepository.Read();
            allRoles.ToList().ForEach(r =>
            {
                var role = new RoleInfo
                {
                    Id = r.Id,
                    Name = r.Name
                };
                var permissions = permissionRepository.GetAllByRoleId(r.Id);
                if (permissions != null)
                {
                    var resources = new List<Resource>();
                    permissions.ToList().ForEach(p =>
                    {
                        var resource = resourceRepository.GetById(p.ResourceId);
                        if (resource != null)
                        {
                            resources.Add(resource);
                        }
                    });
                    role.Resources = resources.ToArray();
                }
                result.Add(role);
            });

            return result;
        }

        public async Task AddRole(RoleInfo roleInfo)
        {
            if (roleInfo == null || roleInfo.Resources == null || roleInfo.Resources.Length == 0)
            {
                throw new BadRequestException("Invalid role information!");
            }
            if (roleRepository.Read().Any(r => r.Id == roleInfo.Id || r.Name == roleInfo.Name))
            {
                throw new BadRequestException("Duplicated role!");
            }
            if (roleInfo.Resources.ToList().Any(r => !resourceRepository.Read().Any(i => i.Id == r.Id)))
            {
                throw new BadRequestException("Invalid resource!");
            }
            await roleRepository.Create(new Role
            {
                Id = roleInfo.Id,
                Name = roleInfo.Name
            });
            roleInfo.Resources.ToList().ForEach(r =>
            {
                permissionRepository.Create(new Permission
                {
                    RoleId = roleInfo.Id,
                    ResourceId = r.Id
                });
            });
        }

        public void UpdateRole(RoleInfo roleInfo)
        {
            if (roleInfo == null || roleInfo.Resources == null || roleInfo.Resources.Length == 0)
            {
                throw new BadRequestException("Invalid role information!");
            }
            if (!roleRepository.Read().Any(r => r.Id == roleInfo.Id))
            {
                throw new BadRequestException("Role doesn't exist!");
            }
            if (roleInfo.Resources.ToList().Any(r => !resourceRepository.Read().Any(i => i.Id == r.Id)))
            {
                throw new BadRequestException("Invalid resource!");
            }
            try
            {
                permissionRepository.GetAllByRoleId(roleInfo.Id).ToList().ForEach(p =>
                {
                    permissionRepository.Delete(p);
                });
                roleInfo.Resources.ToList().ForEach(r =>
                {
                    permissionRepository.Create(new Permission
                    {
                        RoleId = roleInfo.Id,
                        ResourceId = r.Id
                    });
                });
            }
            catch
            {
                throw new InternalException("DB error occurred when updating role.");
            }
        }

        public async Task<Role> AddRole(Role role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.Name))
            {
                throw new BadRequestException("Role name cannot be empty!");
            }
            if (roleRepository.Read().Any(r => r.Name == role.Name))
            {
                throw new BadRequestException("Duplicated role name!");
            }
            try
            {
                return await roleRepository.Create(role);
            }
            catch
            {
                throw new InternalException("DB error occurred when adding a role.");
            }
        }

        public async Task<Permission> AddPermission(Permission permission)
        {
            if (!roleRepository.Read().Any(r => r.Id == permission.RoleId))
            {
                throw new BadRequestException("Role doesn't exist!");
            }
            if (!resourceRepository.Read().Any(i => i.Id == permission.ResourceId))
            {
                throw new BadRequestException("Invalid resource!");
            }
            if (permissionRepository.Read().Any(p => p.RoleId == permission.RoleId && p.ResourceId == permission.ResourceId))
            {
                throw new BadRequestException("Duplicated permission!");
            }
            try
            {
                return await permissionRepository.Create(permission);
            }
            catch
            {
                throw new InternalException("DB error occurred when adding a permission.");
            }
        }

        public async Task<int> DeleteRole(Guid id)
        {
            if (!roleRepository.Read().Any(r => r.Id == id))
            {
                throw new BadRequestException("Role doesn't exist!");
            }
            try
            {
                var role = roleRepository.GetById(id);
                var count = await roleRepository.Delete(role);
                permissionRepository.GetAllByRoleId(id).ToList().ForEach(p =>
                {
                    permissionRepository.Delete(p);
                });
                return count;
            }
            catch
            {
                throw new InternalException("DB error occurred when updating role.");
            }
        }

        public async Task<int> DeletePermission(Permission permission)
        {
            try
            {
                var current = permissionRepository
                    .Read()
                    .Where(r => r.RoleId == permission.RoleId && r.ResourceId == permission.ResourceId)
                    .FirstOrDefault();
                if (current != null)
                {
                    return await permissionRepository.Delete(current);

                }
                else
                {
                    throw new BadRequestException("Permission doesn't exist!");
                }
            }
            catch
            {
                throw new InternalException("DB error occurred when deleting role.");
            }
        }

        public IEnumerable<Resource> GetAllResources()
        {
            return resourceRepository.Read();
        }
    }
}