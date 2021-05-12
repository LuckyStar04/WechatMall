using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WechatMall.Api.Dtos;
using WechatMall.Api.Helpers;
using WechatMall.Api.Services;

namespace WechatMall.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly IHttpClientFactory clientFactory;
        private readonly IMemoryCache cache;

        private static TimeSpan CacheExpireSpan = new TimeSpan(1, 0, 0, 0);

        public AccountController(IConfiguration configuration,
                                 IUserRepository userRepository,
                                 IHttpClientFactory clientFactory,
                                 IMemoryCache cache)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.clientFactory = clientFactory;
            this.cache = cache;
        }

        [AllowAnonymous]
        [HttpPost("admin/login")]
        public IActionResult AdminLogin([FromBody] AdminLoginDto loginDto)
        {
            //User Authentication
            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("Email or Password can not be empty");
            }
            
            //因为Admin用户只有一个，也不提供注册功能，想想还是不用存数据库了
            var username = configuration["AdminUser:UserName"];
            var password = configuration["AdminUser:Password"];
            if (!username.Equals(loginDto.Username) || !password.Equals(loginDto.Password))
            {
                return Unauthorized();
            }
            
            //比对通过即可发Token。
            var token = Jwt.GenerateJWT(configuration, username, "Admin");

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("users/login")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginDto loginDto)
        {
            var session = await GetSessionKey(loginDto.Code);
            if (session.openid == null)
            {
                return BadRequest();
            }
            var user = await userRepository.GetUserByOpenIDAsync(session.openid);
            if (user == null)
            {
                cache.Set("Session_" + session.openid, session.session_key, CacheExpireSpan);
                cache.Set("UnionID_" + session.openid, session.unionid, CacheExpireSpan);
                return Unauthorized(session.openid);
            }
            user.SessionKey = session.session_key;
            await userRepository.SaveAsync();

            var token = Jwt.GenerateJWT(configuration, user.UserID.ToString(), "User");
            var jsonToReturn = JsonSerializer.Serialize(
                new
                {
                    accessToken = token,
                    userid = user.UserID
                });
            return Ok(jsonToReturn);
        }

        private async Task<WxSession> GetSessionKey(string code)
        {
            var appid = configuration["MiniApp:AppID"];
            var secret = configuration["MiniApp:AppSecret"];
            var url = configuration["MiniApp:AuthUrl"];

            url = string.Format(url, appid, secret, code.Trim());

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<WxSession>(responseStream);
            }
            else
            {
                return null;
            }
        }
    }

    public class WxSession
    {
        public string openid { get; set; }
        public string unionid { get; set; }
        public string session_key { get; set; }
    }
}
