using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public interface IFareRepository
    {
        Task<ShippingFare> GetFare(int id);
        void AddFare(ShippingFare fare);
        void UpdateFare(ShippingFare fare);
        void DeleteFare(ShippingFare fare);
        Task<bool> SaveAsync();
    }
}
