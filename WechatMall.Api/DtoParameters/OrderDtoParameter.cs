using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Entities;

namespace WechatMall.Api.DtoParameters
{
    public class OrderDtoParameter
    {
        private const int MinPageSize = 5;
        private const int MaxPageSize = 20;
        public Guid? UserID { get; set; }
        public OrderStatus? Status { get; set; }
        public int PageNumber
        {
            get => _PageNumber;
            set => _PageNumber = (value < 1 ? 1 : value);
        }
        private int _PageNumber = 1;
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value < MinPageSize ? MinPageSize : (value > MaxPageSize ? MaxPageSize : value));
        }
        private int _PageSize = 5;
    }
}
