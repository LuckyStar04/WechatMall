using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByID(string orderID);
        Task<IEnumerable<Order>> GetOrders(Guid userID);

        void AddOrder(Guid userID, Order order);
        void UpdateOrder(Order order);
        void RemoveOrder(Order order);

        Task<bool> SaveAsync();
    }
}
