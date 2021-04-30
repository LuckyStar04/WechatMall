using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Data;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MallDbContext context;

        public OrderRepository(MallDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Order> GetQueryableOrder()
        {
            return context.Orders;
        }

        public async Task<Order> GetOrderByID(string orderID)
        {
            return await context.Orders.Include(o => o.OrderItems)
                                       .Where(o => o.OrderID.Equals(orderID))
                                       .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetOrders(Guid userID)
        {
            return await context.Orders.Include(o => o.OrderItems)
                                       .ThenInclude(o => o.Product)
                                       .ThenInclude(p => p.Images)
                                       .Where(o => o.UserID.Equals(userID))
                                       .ToListAsync();
        }

        public void AddOrder(Guid userID, Order order)
        {
            if (userID.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(userID));
            if (order == null) throw new ArgumentNullException(nameof(order));

            order.UserID = userID;
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.OrderID = order.OrderID;
                context.OrderItems.Add(orderItem);
            }
            context.Orders.Add(order);
        }

        public void UpdateOrder(Order order)
        {
            //no need write code
        }

        public void RemoveOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            foreach (var orderItem in order.OrderItems)
            {
                context.OrderItems.Remove(orderItem);
            }
            context.Orders.Remove(order);
        }

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
