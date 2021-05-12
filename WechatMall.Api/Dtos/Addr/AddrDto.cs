using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WechatMall.Api.Dtos
{
    public class AddrDto
    {
        public int Id { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ReceiverName { get; set; }
        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
