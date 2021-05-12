using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;
using WechatMall.Api.Helpers;
using WechatMall.Api.Services;

namespace WechatMall.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        public UserController(IUserRepository userRepository,
                              IConfiguration configuration,
                              IMapper mapper,
                              IMemoryCache cache)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
            this.mapper = mapper;
            this.cache = cache;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{userid:guid}", Name = nameof(GetUser))]
        public async Task<ActionResult<UserDto>> GetUser(Guid userid)
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role.Equals("User"))
            {
                Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                if (!userID.Equals(userid))
                {
                    return Unauthorized();
                }
            }

            var userEntity = await userRepository.GetUserAsync(userid);
            if (userEntity == null)
            {
                return NotFound();
            }
            var dtoToReturn = mapper.Map<UserDto>(userEntity);
            return dtoToReturn;
        }

        //[Authorize(Roles = "Admin,User")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserAddDto user)
        {
            var sessionKey = "Session_" + user.OpenID;
            var unionKey = "UnionID_" + user.OpenID;
            string session = string.Empty;
            if (!cache.TryGetValue(sessionKey, out session))
            {
                return NotFound(nameof(user.OpenID));
            }
            string unionid = cache.Get<string>(unionKey);
            cache.Remove(sessionKey);
            //var decryptedData = WXEncrypt.AESDecrypt(user.EncryptedData, session_key, user.Iv);

            User newUser = new User
            {
                OpenID = user.OpenID,
                UnionID = unionid,
                SessionKey = session,
                NickName = user.UserInfo.NickName,
                Gender = user.UserInfo.Gender,
                Language = user.UserInfo.Language,
                City = user.UserInfo.City,
                Province = user.UserInfo.Province,
                Country = user.UserInfo.Country,
                AvatarUrl = user.UserInfo.AvatarUrl
            };
            userRepository.AddUser(newUser);
            await userRepository.SaveAsync();
            var token = Jwt.GenerateJWT(configuration, newUser.UserID.ToString(), "User");
            var jsonToReturn = JsonSerializer.Serialize(
                new
                {
                    accessToken = token,
                    userid = newUser.UserID
                });
            return Ok(jsonToReturn);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("{userid:guid}")]
        public async Task<IActionResult> UpdateUser(Guid userid, UserUpdateDto user)
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role.Equals("User"))
            {
                Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                if (!userID.Equals(userid))
                {
                    return Unauthorized();
                }
            }

            var userEntity = await userRepository.GetUserAsync(userid);
            if (userEntity == null)
            {
                return NotFound();
            }
            mapper.Map(user, userEntity);
            userRepository.UpdateUser(userEntity);
            await userRepository.SaveAsync();
            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{userid}")]
        //public async Task<IActionResult> DeleteUser(Guid userid)
        //{
        //    var userEntity = await userRepository.GetUserAsync(userid);
        //    if (userEntity == null)
        //    {
        //        return NotFound();
        //    }
        //    userRepository.DeleteUser(userEntity);
        //    await userRepository.SaveAsync();
        //    return NoContent();
        //}
    }
}
