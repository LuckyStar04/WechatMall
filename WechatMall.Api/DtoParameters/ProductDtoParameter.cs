﻿namespace WechatMall.Api.DtoParameters
{
    public class ProductDtoParameter
    {
        private const int MinPageSize = 5;
        private const int MaxPageSize = 20;
        public string CategoryID { get; set; }
        public OrderType OrderBy { get; set; } = OrderType.None;
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

    public enum OrderType
    {
        None = 0,
        Recommend = 1,
        TopSales = 2
    }
}
