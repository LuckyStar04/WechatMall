using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WechatMall.Api.Data;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly MallDbContext context;

        public ConfigRepository(MallDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> ConfigExistsAsync(string key)
        {
            return await context.Configs.AnyAsync(c => c.Key.Equals(key));
        }

        public async Task<SiteConfig> GetConfig(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return await context.Configs.FirstOrDefaultAsync(c => c.Key.Equals(key));
        }

        public void AddConfig(SiteConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            context.Configs.Add(config);
        }

        public void UpdateConfig(SiteConfig config)
        {
            //no need write code
        }

        public void RemoveConfig(SiteConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            context.Configs.Remove(config);
        }

        public async Task<bool> SaveAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }
    }
}
