using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public interface IAddrRepository
    {
        Task<IEnumerable<ShippingAddr>> GetAddrsAsync(Guid userid);
        Task<ShippingAddr> GetDefaultAddr(Guid userid);
        Task<ShippingAddr> GetAddr(int addrId);
        void AddAddr(Guid userid, ShippingAddr addr);
        void UpdateAddr(ShippingAddr addr);
        void DeleteAddr(ShippingAddr addr);
        Task<bool> SaveAsync();
    }
}
