using WechatMall.Api.Entities;

namespace WechatMall.Api.Dtos
{
    public class UserAddDto
    {
        public string OpenID { get; set; }
        public UserInfo UserInfo { get; set; }
        public string EncryptedData { get; set; }
        public string Iv { get; set; }
    }

    public class UserInfo
    {
        public string NickName { get; set; }
        public string AvatarUrl { get; set; }
        public Gender Gender { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }
}
