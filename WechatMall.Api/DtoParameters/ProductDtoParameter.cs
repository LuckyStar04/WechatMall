namespace WechatMall.Api.DtoParameters
{
    /// <summary>
    /// 商品查询的总参数
    /// </summary>
    public class ProductDtoParameter
    {
        private const int MinPageSize = 5;
        private const int MaxPageSize = 20;

        /// <summary>
        /// 商品的分类ID
        /// </summary>
        public string CategoryID { get; set; }

        /// <summary>
        /// 商品的排序参数
        /// </summary>
        public OrderType OrderBy { get; set; } = OrderType.None;

        /// <summary>
        /// 要查看的具体页数
        /// </summary>
        public int PageNumber
        {
            get => _PageNumber;
            set => _PageNumber = (value < 1 ? 1 : value);
        }
        private int _PageNumber = 1;

        /// <summary>
        /// 要查看的分页大小
        /// </summary>
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value < MinPageSize ? MinPageSize : (value > MaxPageSize ? MaxPageSize : value));
        }
        private int _PageSize = 5;
    }

    /// <summary>
    /// 商品的排序参数enum
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 无需排序
        /// </summary>
        None = 0,
        /// <summary>
        /// 按系统推荐度排序
        /// </summary>
        Recommend = 1,
        /// <summary>
        /// 按销量从高到低排序
        /// </summary>
        TopSales = 2
    }
}
