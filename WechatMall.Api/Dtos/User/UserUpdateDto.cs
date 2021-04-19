using System;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Dtos
{
    public class UserUpdateDto
    {
        public Guid UserID { get; set; }
        public string OpenID { get; set; }
        public string NickName { get; set; }
        public Gender Gender { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string AvatarUrl { get; set; }
    }
}
