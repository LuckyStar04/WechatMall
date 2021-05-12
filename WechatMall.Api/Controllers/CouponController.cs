using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WechatMall.Api.DtoParameters;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;
using WechatMall.Api.Services;

namespace WechatMall.Api.Controllers
{
    [ApiController]
    [Route("api/coupons")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository couponRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public CouponController(ICouponRepository couponRepository,
                                IUserRepository userRepository,
                                IMapper mapper)
        {
            this.couponRepository = couponRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Name = nameof(GetCoupons))]
        public async Task<ActionResult<IEnumerable<CouponDto>>> GetCoupons([FromQuery] CouponDtoParameter parameter)
        {
            var now = DateTime.Now;
            IQueryable<Coupon> coupons = couponRepository.GetIQueryableCoupon();
            switch (parameter.Type)
            {
                case null:
                    break;
                case CouponsType.Expired:
                    coupons = coupons.Where(c => c.EndTime < now && !c.IsDeleted);
                    break;
                case CouponsType.Available:
                    coupons = coupons.Where(c => c.StartTime <= now && c.EndTime >= now && !c.IsDeleted);
                    break;
                case CouponsType.Deleted:
                    coupons = coupons.Where(c => c.IsDeleted);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter.Type));
            }

            coupons = coupons.OrderBy(c => c.StartTime);

            var dtoToReturn = mapper.Map<IEnumerable<CouponDto>>(await coupons.ToListAsync());
            return Ok(dtoToReturn);
        }

        [Authorize(Roles = "User")]
        [HttpGet("counts")]
        public async Task<ActionResult<CouponCountDto>> GetCouponCounts()
        {
            var now = DateTime.Now;
            Guid UserID = new(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            IQueryable<Coupon> coupons = couponRepository.GetIQueryableCoupon();
            IQueryable<Coupon_User> couponUser = couponRepository.GetIQueryableCouponUser()
                                                                 .Include(c => c.Coupon);

            var result = new int[2];
            result[0] = await coupons.Where(c => c.StartTime <= now && c.EndTime >= now && !c.IsDeleted).CountAsync();
            result[1] = await couponUser.Where(c => c.UserID.Equals(UserID) && c.Coupon.StartTime <= now && c.Coupon.EndTime >= now && !c.Coupon.IsDeleted).SumAsync(c => c.RemainedCount);
            return Ok(new CouponCountDto { CouponCounts = result });
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{couponID}", Name = nameof(GetCoupon))]
        public async Task<ActionResult<CouponDto>> GetCoupon(int couponID)
        {
            var coupon = await couponRepository.GetCouponAsync(couponID);
            var dtoToReturn = mapper.Map<CouponDto>(coupon);
            return Ok(dtoToReturn);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCoupon(CouponAddDto coupon)
        {
            if (coupon.ProductIDs == null) coupon.ProductIDs = "";
            var couponEntity = mapper.Map<Coupon>(coupon);
            couponRepository.AddCoupon(couponEntity);
            await couponRepository.SaveAsync();

            var dtoToReturn = mapper.Map<CouponDto>(couponEntity);
            return CreatedAtRoute(nameof(GetCoupon),
                                  new { couponID = couponEntity.Id },
                                  dtoToReturn);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{couponID}")]
        public async Task<IActionResult> UpdateCoupon(int couponID, CouponUpdateDto coupon)
        {
            var couponEntity = await couponRepository.GetCouponAsync(couponID);
            if (couponEntity == null)
            {
                return NotFound();
            }
            mapper.Map(coupon, couponEntity);
            couponRepository.UpdateCoupon(couponEntity);
            await couponRepository.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{couponID}")]
        public async Task<IActionResult> PartiallyUpdateCoupon(int couponID, JsonPatchDocument<CouponUpdateDto> patchDocument)
        {
            var couponEntity = await couponRepository.GetCouponAsync(couponID);
            if (couponEntity == null)
            {
                return NotFound();
            }

            var dtoToPatch = mapper.Map<CouponUpdateDto>(couponEntity);
            patchDocument.ApplyTo(dtoToPatch, ModelState);
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(dtoToPatch, couponEntity);
            couponRepository.UpdateCoupon(couponEntity);
            await couponRepository.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{couponID}")]
        public async Task<IActionResult> DeleteCoupon(int couponID)
        {
            var coupon = await couponRepository.GetCouponAsync(couponID);
            if (coupon == null) return NotFound();

            couponRepository.DeleteCoupon(coupon);
            await couponRepository.SaveAsync();
            return NoContent();
        }
    }
}
