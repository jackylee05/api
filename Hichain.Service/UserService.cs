using Hichain.Common.Utilities;
using Hichain.DataAccess.Data.Repository;
using Hichain.Entity;
using Hichain.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hichain.Services
{
    public class UserService:RepositoryFactory
    {
        public async Task CreateUserAsync(User entity)
        {
            var db = await this.BaseRepository().BeginTrans();
            try
            {
                await db.Insert(entity);
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }
        public async Task<User> GetEntity(string username)
        {
            var exp = LinqExtensions.True<User>();
            exp = exp.And(r => r.Username == username);
            var userlist = await GetList(exp);
            return userlist.FirstOrDefault();
        }
        public async Task<List<User>> GetList(Expression<Func<User, bool>> paramFilter)
        {
            var list = await this.BaseRepository().FindList(paramFilter);
            return list.ToList();
        }
    }
}
