using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(Guid userid);
        Task<bool> UserExistsAsync(Guid userid);
        Task<User> GetUserByOpenIDAsync(string openid);
        Task<bool> UserExistsByOpenIDAsync(string openid);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Task<bool> SaveAsync();
    }
}
