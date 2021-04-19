using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Data;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly MallDbContext context;

        public UserRepository(MallDbContext context)
        {
            this.context = context;
        }
        public async Task<User> GetUserAsync(Guid userid)
        {
            return await context.Users.Where(u => u.UserID.Equals(userid)).FirstOrDefaultAsync();
        }

        public async Task<bool> UserExistsAsync(Guid userid)
        {
            return await context.Users.AnyAsync(u => u.UserID.Equals(userid));
        }

        public async Task<User> GetUserByOpenIDAsync(string openid)
        {
            return await context.Users.Where(u => u.OpenID.Equals(openid)).FirstOrDefaultAsync();
        }

        public async Task<bool> UserExistsByOpenIDAsync(string openid)
        {
            return await context.Users.AnyAsync(u => u.OpenID.Equals(openid));
        }

        public void AddUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.UserID = Guid.NewGuid();
            context.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            //no need code
        }

        public void DeleteUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            context.Users.Remove(user);
        }

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
