using System;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public interface ICouponRepository
    {
        IQueryable<Coupon> GetIQueryableCoupon();
        Task<bool> CouponExistsAsync(int couponID);
        Task<Coupon> GetCouponAsync(int couponID);

        void AddCoupon(Coupon coupon);
        void UpdateCoupon(Coupon coupon);
        void DeleteCoupon(Coupon coupon);

        IQueryable<Coupon_User> GetIQueryableCouponUser();
        Task<Coupon_User> GetCouponUserAsync(int couponID, Guid userID);
        Task<Coupon_User> AddCouponToUserAsync(int couponID, Guid userID);
        void AddCouponUser(Coupon_User coupon_user);
        void UpdateCouponUser(Coupon_User coupon_user);
        void DeleteCouponUser(Coupon_User coupon_user);
        Task<bool> SaveAsync();
    }
}
