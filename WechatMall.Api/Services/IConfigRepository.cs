using System.Collections.Generic;
using System.Threading.Tasks;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public interface IConfigRepository
    {
        Task<bool> ConfigExistsAsync(string key);
        Task<SiteConfig> GetConfig(string key);
        void AddConfig(SiteConfig config);
        void UpdateConfig(SiteConfig config);
        void RemoveConfig(SiteConfig config);
        Task<bool> SaveAsync();
    }
}
