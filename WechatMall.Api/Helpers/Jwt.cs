using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WechatMall.Api.Helpers
{
    public class Jwt
    {
        public static string GenerateJWT(IConfiguration configuration, string sub, string role)
        {
            var algorithm = SecurityAlgorithms.HmacSha256;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, sub),
                new Claim(ClaimTypes.Role, role),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));

            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            var now = DateTime.Now;

            var token = new JwtSecurityToken(
                configuration["JWT:Issuer"],   //Issuer
                configuration["JWT:Audience"], //Audience
                claims,                        //Claims,
                now,                           //notBefore
                now.AddDays(2).AddHours(23),   //expires: 不大于微信session_key的3天有效期
                signingCredentials
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
