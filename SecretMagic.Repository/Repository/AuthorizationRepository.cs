using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecretMagic.Model;

namespace SecretMagic.Repository
{

    public class AuthorizationRepository : IAuthorizationRepository
    {
        protected readonly DataContext context;

        public AuthorizationRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<string[]> GetUserResource(string userId)
        {
            var result = new List<string>();
            DbConnection conn = null;
            const string query = @"select resources.Name from users 
                inner join urm on users.Id = urm.UserId
                inner join roles on urm.RoleId = roles.Id
                inner join permissions on roles.Id = permissions.RoleId
                inner join resources on permissions.ResourceId = resources.Id
                where users.Id = '{0}'";
            try
            {
                var search = string.Format(query, userId);
                conn = context.Database.GetDbConnection();
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = search;
                command.CommandType = CommandType.Text;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        result.Add(reader.GetString(0));
                    }
                }
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result.ToArray();
        }
    }
}