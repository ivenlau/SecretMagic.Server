using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Commom;
using SecretMagic.Model;
using SecretMagic.Repository;

namespace SecretMagic.API.Services
{
    public class LoginService: ILoginService
    {
        private readonly ILogger<LoginService> logger;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUrmRepository urmRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IResourceRepository resourceRepository;
        private readonly ITokenService tokenService;

        public LoginService (ILogger<LoginService> logger,
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
        
        public async Task<LoginResult> Login(string userName, string password)
        {
            LoginResult result = null;
            var sha = new SHA256Managed();
            var encryptedPass = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
            sha.Clear();
            var user = await userRepository.ValidateUser(userName, Encoding.ASCII.GetString(encryptedPass));
            if(user == null)
            {
                throw new BadRequestException("Invalid credential!");
            }
            var urm = await urmRepository.GetUrmByUserId(user.Id);
            if(urm == null)
            {
                throw new BadRequestException("No active role for this user!");
            }
            var role = roleRepository.GetById(urm.RoleId);
            if(role == null)
            {
                throw new InternalException("Invalid user role data!");
            }

            IQueryable<Permission> permissions= permissionRepository.GetAllByRoleId(role.Id);
            if (permissions == null)
            {
                throw new BadRequestException("No active permission for this user!");
            }
            var resources= new List<Resource>();
            var resourceNames = new List<string>();
            permissions.ToList().ForEach(p =>{
                var resource = resourceRepository.GetById(p.ResourceId);
                if(resource == null)
                {
                    throw new BadRequestException("No active permission for this user!");
                }
                resources.Add(resource);
                resourceNames.Add(resource.Name);
            });

            var token = tokenService.CreateToken(user.Id.ToString(), userName, role.Name, resourceNames.ToArray());
            result = new LoginResult();
            result.Token = token;
            result.Role = role.Name; 
            result.User = user;
            result.Resource = resourceNames.ToArray();

            return result;
        }
    }
}