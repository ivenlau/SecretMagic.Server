using System;
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
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> logger;
        private readonly IRoleRepository roleRepository;
        private readonly IUserRepository userRepository;
        private readonly IUrmRepository urmRepository;

        public UserService(ILogger<UserService> logger,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IUrmRepository urmRepository
            )
        {
            this.logger = logger;
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
            this.urmRepository = urmRepository;
        }

        public async Task AddUser(UserInfo userInfo)
        {
            if (userInfo == null || string.IsNullOrWhiteSpace(userInfo.Name))
            {
                throw new BadRequestException("Invalid user information!");
            }
            var role = this.roleRepository.GetById(userInfo.RoleId);
            if (role == null)
            {
                throw new BadRequestException("Incorrect role information!");
            }
            try
            {
                var encryptedPass = MD5Encoding(userInfo.Password);
                var user = await this.userRepository.Create(new User
                {
                    Name = userInfo.Name,
                    Email = userInfo.Email,
                    Password = encryptedPass
                });
                await this.urmRepository.Create(new UserRoleMapping
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
            }
            catch
            {
                throw new InternalException("DB error occurred when adding a user.");
            }
        }

        public async Task<int> DeleteUser(Guid id)
        {
            var user = userRepository.GetById(id);
            var urm = await this.urmRepository.GetUrmByUserId(id);
            if (urm == null || user == null)
            {
                throw new BadRequestException("Invalid User Id!");
            }
            try
            {
                await this.urmRepository.Delete(urm);

                return await this.userRepository.Delete(user);
            }
            catch
            {
                throw new InternalException("DB error occurred when deleting a user.");
            }
        }

        public IEnumerable<UserInfo> GetAllUsers()
        {
            var result = new List<UserInfo>();
            try
            {
                this.userRepository.Read().ToList().ForEach(u =>
                {
                    var userInfo = new UserInfo
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email
                    };
                    var urm = this.urmRepository.GetUrmByUserId(u.Id).Result;
                    if (urm != null)
                    {
                        var role = this.roleRepository.GetById(urm.RoleId);
                        if (role != null)
                        {
                            userInfo.RoleId = role.Id;
                            userInfo.Role = role.Name;
                        }
                    }
                    result.Add(userInfo);
                });
            }
            catch
            {
                throw new InternalException("DB error occurred when get all users.");
            }

            return result;
        }

        public async Task UpdateUser(UserInfo userInfo)
        {
            try
            {
                if (userInfo == null ||
                string.IsNullOrWhiteSpace(userInfo.Name))
                {
                    throw new BadRequestException("Incorrect user information!");
                }

                var user = this.userRepository.GetById(userInfo.Id);
                if (user == null)
                {
                    throw new BadRequestException("Incorrect user id!");
                }
                user.Name = userInfo.Name;
                user.Email = userInfo.Email;
                await this.userRepository.Update(user);

                var role = this.roleRepository.GetById(userInfo.RoleId);
                if (role == null)
                {
                    throw new BadRequestException("Incorrect role id!");
                }
                var urm = await this.urmRepository.GetUrmByUserId(user.Id);
                if (urm == null)
                {
                    await this.urmRepository.Create(new UserRoleMapping
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    });
                }
                else
                {
                    urm.RoleId = role.Id;
                    await this.urmRepository.Update(urm);
                }
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new InternalException("DB error occurred when updating a user.");
            }
        }

        private static string MD5Encoding(string rawPass)
        {
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(rawPass);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder stb = new StringBuilder();
            foreach (byte b in hs)
            {
                stb.Append(b.ToString("x2"));
            }
            return stb.ToString();
        }
    }
}