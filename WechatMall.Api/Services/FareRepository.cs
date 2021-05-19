using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Data;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public class FareRepository : IFareRepository
    {
        private readonly MallDbContext context;

        public FareRepository(MallDbContext context)
        {
            this.context = context;
        }

        public async Task<ShippingFare> GetFare(int id)
        {
            return await context.ShippingFares.FindAsync(id);
        }

        public void AddFare(ShippingFare fare)
        {
            if (fare == null) throw new ArgumentNullException(nameof(fare));

            context.ShippingFares.Add(fare);
        }

        public void UpdateFare(ShippingFare fare)
        {
            //no need write code.
        }

        public void DeleteFare(ShippingFare fare)
        {
            if (fare == null) throw new ArgumentNullException(nameof(fare));

            context.ShippingFares.Remove(fare);
        }

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
