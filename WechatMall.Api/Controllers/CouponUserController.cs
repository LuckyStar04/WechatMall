using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/users/coupons")]
    public class CouponUserController : ControllerBase
    {
        private readonly ICouponRepository couponRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public CouponUserController(ICouponRepository couponRepository,
                                    IUserRepository userRepository,
                                    IMapper mapper)
        {
            this.couponRepository = couponRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [Authorize(Roles = "User")]
        [HttpGet(Name = nameof(GetCouponsOfUser))]
        public async Task<ActionResult<IEnumerable<CouponUserDto>>> GetCouponsOfUser([FromQuery] CouponUserDtoParameter parameter)
        {
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (!await userRepository.UserExistsAsync(userID))
            {
                return NotFound("用户不存在!");
            }
            IEnumerable<Coupon_User> result;
            IQueryable<Coupon_User> queryable = couponRepository.GetIQueryableCouponUser()
                                                                  .Include(c => c.Coupon);

            DateTime now = DateTime.Now;
            switch (parameter.Type)
            {
                case CouponsType.Available:
                    var coupons = await couponRepository.GetIQueryableCoupon().Include(c => c.Users).ThenInclude(u => u.Coupon).Where(c => c.StartTime <= now && c.EndTime >= now && !c.IsDeleted).OrderBy(c => c.Id).ToListAsync();
                    List<Coupon_User> couponUsers = new(16);
                    foreach (var coupon in coupons)
                    {
                        var u = coupon.Users.Where(u => u.UserID.Equals(userID))
                                            .FirstOrDefault();

                        if (u != null) couponUsers.Add(u);
                        else couponUsers.Add(new Coupon_User
                        {
                            UserID = userID,
                            CouponID = coupon.Id,
                            Coupon = await couponRepository.GetCouponAsync(coupon.Id),
                            RecievedCount = 0,
                            RemainedCount = 0
                        });
                    }
                    result = couponUsers;
                    break;
                case CouponsType.Expired:
                    result = await queryable.Where(c => c.UserID.Equals(userID)
                                                     && c.Coupon.EndTime < now
                                                     && !c.Coupon.IsDeleted)
                                            .ToListAsync();
                    break;
                case CouponsType.Owned:
                    result = await queryable.Where(c => c.UserID.Equals(userID)
                                                     && c.Coupon.StartTime <= now
                                                     && c.Coupon.EndTime >= now)
                                            .ToListAsync();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter.Type));
            }

            var dtoToReturn = mapper.Map<IEnumerable<CouponUserDto>>(result);
            return Ok(dtoToReturn);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{couponID:int:min(1)}", Name = nameof(GetCouponOfUser))]
        public async Task<ActionResult<CouponUserDto>> GetCouponOfUser(int couponID)
        {
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var couponUser = await couponRepository.GetCouponUserAsync(couponID, userID);
            if (couponUser == null)
            {
                return NotFound();
            }

            var dtoToReturn = mapper.Map<CouponUserDto>(couponUser);
            return Ok(dtoToReturn);
        }

        [Authorize(Roles = "User")]
        [HttpPost("{couponID:int:min(1)}")]
        public async Task<IActionResult> AddCouponToUser(int couponID)
        {
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (!await userRepository.UserExistsAsync(userID))
            {
                return UnprocessableEntity("User not exist!");
            }
            var coupon = await couponRepository.GetCouponAsync(couponID);
            if (coupon == null || coupon.IsDeleted)
            {
                return UnprocessableEntity("Coupon not exist!");
            }
            if (coupon.CouponCount <= 0)
            {
                return BadRequest("Coupon is not enough");
            }

            var couponUser = await couponRepository.AddCouponToUserAsync(couponID, userID);
            if (couponUser == null)
            {
                return Conflict("Get coupon failed, maybe try again later");
            }

            var dtoToReturn = mapper.Map<CouponUserDto>(couponUser);
            return CreatedAtRoute(nameof(GetCouponOfUser), new { couponID, userID }, dtoToReturn);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("{couponID:int:min(1)}")]
        public async Task<IActionResult> UpdateCouponUser(int couponID, CouponUserUpdateDto couponUser)
        {
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var couponUserEntity = await couponRepository.GetCouponUserAsync(couponID, userID);
            if (couponUserEntity == null)
            {
                return NotFound();
            }

            mapper.Map(couponUser, couponUserEntity);
            couponRepository.UpdateCouponUser(couponUserEntity);
            await couponRepository.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{couponID:int:min(1)}")]
        public async Task<IActionResult> DeleteCouponUser(int couponID)
        {
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var couponUserEntity = await couponRepository.GetCouponUserAsync(couponID, userID);
            if (couponUserEntity == null)
            {
                return NotFound();
            }
            couponRepository.DeleteCouponUser(couponUserEntity);
            await couponRepository.SaveAsync();
            return NoContent();
        }
    }
}
