using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Text;

namespace WechatMall.Api.Helpers
{
    public class WXEncrypt
    {
        #region SHA1解密

        /// <summary>
        /// 解密小程序的encryptedData
        /// </summary>
        /// <param name="encryptedData">加密的信息</param>
        /// <param name="sessionKey">key</param>
        /// <param name="iv">加密算法的初始向量</param>
        public static WxPhoneModel DescodeWxSHA1(string encryptedData, string sessionKey, string iv)
        {
            WxPhoneModel model = null;
            var res = AESDecrypt(encryptedData, sessionKey, iv);// {"phoneNumber":"152XXXX9583","purePhoneNumber":"1525XXXX3","countryCode":"86","watermark":{"timestamp":1525829586,"appid":"wx38XXXXXXXX43"}} 
            if (!string.IsNullOrEmpty(res))
            {
                model = JObject.Parse(res).ToObject<WxPhoneModel>();
            }
            return model;
        }

        public static string AESDecrypt(string encryptedData, string sessionKey, string iv)
        {
            try
            {
                //16进制数据转换成byte
                var encryptedDataByte = Convert.FromBase64String(encryptedData);  // strToToHexByte(text);
                var rijndaelCipher = new RijndaelManaged
                {
                    Key = Convert.FromBase64String(sessionKey),
                    IV = Convert.FromBase64String(iv),
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };

                var transform = rijndaelCipher.CreateDecryptor();
                var plainText = transform.TransformFinalBlock(encryptedDataByte, 0, encryptedDataByte.Length);
                var result = Encoding.Default.GetString(plainText);

                return result;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
    public class WxPhoneModel
    {
        /// <summary>
        /// 用户绑定的手机号（国外手机号会有区号）
        /// </summary>
        public string PhoneNumber { set; get; }

        /// <summary>
        /// 没有区号的手机号
        /// </summary>
        public string PurePhoneNumber { set; get; }

        /// <summary>
        /// 区号
        /// </summary>
        public string CountryCode { set; get; }

        /// <summary>
        /// 水印
        /// </summary>
        public WaterMarkModel WaterMark { set; get; }
    }

    public class WaterMarkModel
    {
        /// <summary>
        /// appid
        /// </summary>
        public string AppId { set; get; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { set; get; }
    }
}
