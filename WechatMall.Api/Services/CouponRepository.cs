using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Data;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public class CouponRepository : ICouponRepository
    {
        private readonly MallDbContext context;
        private readonly IConfiguration configuration;

        public CouponRepository(MallDbContext context,
                                IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public IQueryable<Coupon> GetIQueryableCoupon()
        {
            return context.Coupons;
        }

        public async Task<bool> CouponExistsAsync(int couponID)
        {
            return await context.Coupons.AnyAsync(c => c.Id.Equals(couponID));
        }

        public async Task<Coupon> GetCouponAsync(int couponID)
        {
            return await context.Coupons.FindAsync(couponID);
        }

        public void AddCoupon(Coupon coupon)
        {
            if (coupon == null) throw new ArgumentNullException(nameof(coupon));
            context.Coupons.Add(coupon);
        }

        public void UpdateCoupon(Coupon coupon)
        {
            //no need code
        }

        public void DeleteCoupon(Coupon coupon)
        {
            if (coupon == null) throw new ArgumentNullException(nameof(coupon));
            context.Coupons.Remove(coupon);
        }
        
        public IQueryable<Coupon_User> GetIQueryableCouponUser()
        {
            return context.Coupon_Users;
        }

        public async Task<Coupon_User> GetCouponUserAsync(int couponID, Guid userID)
        {
            var couponUser = await context.Coupon_Users
                                    .Where(c => c.CouponID == couponID && c.UserID.Equals(userID))
                                    .FirstOrDefaultAsync();
            return couponUser;
        }

        public async Task<Coupon_User> AddCouponToUserAsync(int couponID, Guid userID)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MallDbContext>();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));

            using (MallDbContext mContext = new MallDbContext(optionsBuilder.Options))
            {
                var coupon = await mContext.Coupons.FindAsync(couponID);
                if (coupon == null) return null;
                if (coupon.CouponCount <= 0) return null;

                var couponUser = await mContext.Coupon_Users
                                               .Include(c => c.Coupon)
                                               .Where(c => c.CouponID == couponID
                                                        && c.UserID.Equals(userID))
                                               .FirstOrDefaultAsync();

                if (couponUser == null)
                {
                    coupon.CouponCount--;
                    couponUser = new Coupon_User
                    {
                        CouponID = couponID,
                        UserID = userID,
                        RecievedCount = 1,
                        RemainedCount = 1
                    };
                    mContext.Coupon_Users.Add(couponUser);
                    try
                    {
                        await mContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return null;
                    }
                }
                else
                {
                    if (couponUser.RecievedCount >= coupon.AllowLimit) return null;
                    coupon.CouponCount--;
                    couponUser.RecievedCount++;
                    couponUser.RemainedCount++;
                    try
                    {
                        await mContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return null;
                    }
                }
                return couponUser;
            }
        }

        public void AddCouponUser(Coupon_User coupon_user)
        {
            if (coupon_user == null) throw new ArgumentNullException(nameof(coupon_user));
            context.Coupon_Users.Add(coupon_user);
        }

        public void UpdateCouponUser(Coupon_User coupon_user)
        {
            //no need code
        }

        public void DeleteCouponUser(Coupon_User coupon_user)
        {
            if (coupon_user == null) throw new ArgumentNullException(nameof(coupon_user));
            context.Coupon_Users.Remove(coupon_user);
        }

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
