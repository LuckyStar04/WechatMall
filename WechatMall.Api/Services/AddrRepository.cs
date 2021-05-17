using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Data;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public class AddrRepository : IAddrRepository
    {
        private readonly MallDbContext context;

        public AddrRepository(MallDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ShippingAddr>> GetAddrsAsync(Guid userid)
        {
            return await context.ShippingAddrs.Where(a => a.UserID.Equals(userid)
                                                       && !a.IsDeleted)
                                              .ToListAsync();
        }

        public async Task<ShippingAddr> GetDefaultAddr(Guid userid)
        {
            var addr = await context.ShippingAddrs.Where(a => a.UserID.Equals(userid)
                                                           && a.IsDefault
                                                           && !a.IsDeleted)
                                                  .FirstOrDefaultAsync();
            if (addr == null)
            {
                addr = await context.ShippingAddrs.Where(a => a.UserID.Equals(userid)
                                                           && !a.IsDeleted)
                                                  .OrderBy(a => a.OrderById)
                                                  .FirstOrDefaultAsync();
            }
            return addr;
        }

        public async Task<ShippingAddr> GetAddr(int addrId)
        {
            return await context.ShippingAddrs.FindAsync(addrId);
        }

        public void AddAddr(Guid userid, ShippingAddr addr)
        {
            if (addr == null) throw new ArgumentNullException(nameof(addr));
            addr.UserID = userid;
            if (addr.IsDefault)
            {
                context.Database.ExecuteSqlRaw($"UPDATE `ShippingAddrs` SET IsDefault=0 WHERE UserID='{userid}';");
            }
            context.ShippingAddrs.Add(addr);
        }
        public void UpdateAddr(ShippingAddr addr)
        {
            if (addr.IsDefault)
            {
                context.Database.ExecuteSqlRaw($"UPDATE `ShippingAddrs` SET IsDefault=0 WHERE UserID='{addr.UserID}';");
            }
        }
        public void DeleteAddr(ShippingAddr addr)
        {
            if (addr == null) throw new ArgumentNullException(nameof(addr));
            context.ShippingAddrs.Remove(addr);
        }
        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
