using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WechatMall.Api.DtoParameters
{
    public class CouponDtoParameter
    {
        public CouponsType? Type { get; set; }
    }

    public enum CouponsType
    {
        Expired = 0,
        Available = 1,
        Owned = 2,
        Deleted = 3
    }
}
