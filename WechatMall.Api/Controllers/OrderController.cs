﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using WechatMall.Api.DtoParameters;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;
using WechatMall.Api.Helpers;
using WechatMall.Api.Services;

namespace WechatMall.Api.Controllers
{
    [ApiController]
    [Route("api/users/{userID}/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private static readonly Random random = new Random();

        public OrderController(IOrderRepository orderRepository,
                               IUserRepository userRepository,
                               IProductRepository productRepository,
                               IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(Guid userID)
        {
            var orders = await orderRepository.GetOrders(userID);
            var orderDtos = mapper.Map<IEnumerable<OrderDto>>(orders);

            return Ok(orderDtos);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{orderID}", Name = (nameof(GetOrder)))]
        public async Task<ActionResult<OrderDetailDto>> GetOrder(Guid userID, string orderID)
        {
            var order = await orderRepository.GetOrderByID(orderID);
            if (order == null || !order.UserID.Equals(userID))
            {
                return NotFound();
            }

            var orderDto = mapper.Map<OrderDetailDto>(order);
            return Ok(orderDto);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public async Task<IActionResult> AddOrder(Guid userID, [FromBody] OrderAddDto order)
        {
            var user = userRepository.GetUserAsync(userID);
            if (user == null)
            {
                return NotFound(nameof(userID));
            }

            var itemCount = order.OrderItems.Count();
            if (itemCount == 0)
            {
                return UnprocessableEntity("Order item is empty!");
            }

            var now = DateTime.Now;
            string orderID = $"{(now.Year-2020).ToString().PadLeft(2,'0')}{(now.DayOfYear*24*60+now.Hour*60+now.Minute).ToString().PadLeft(6,'0')}{(user.Id % 10000).ToString().PadLeft(4,'0')}{random.Next(10000).ToString().PadLeft(4,'0')}";

            var orderItems = new List<OrderItem>(itemCount);
            decimal originalPrice = 0m;
            foreach (var addItem in order.OrderItems)
            {
                var product = await productRepository.GetProductAsync(addItem.ProductID);
                var orderItem = new OrderItem()
                {
                    OrderID = orderID,
                    ProductID = addItem.ProductID,
                    Price = product.Price,
                    Amount = addItem.Amount,
                };
                orderItems.Add(orderItem);
                originalPrice += product.Price * addItem.Amount;
            }

            Order orderEntity = new Order()
            {
                UserID = userID,
                OrderID = orderID,
                OrderItems = orderItems,
                Status = "未付款",
                OrderTime = now,
                ShippingAddrId = order.ShippingAddrId,
                CouponAmount = CalcCoupon(),
                OriginalPrice = originalPrice,
                ShippingFare = CalcShippingFare(),
            };
            orderRepository.AddOrder(userID, orderEntity);
            await orderRepository.SaveAsync();
            return CreatedAtRoute(nameof(GetOrder), new { userID, orderID });
        }

        private decimal CalcCoupon()
        {
            throw new NotImplementedException();
        }

        private decimal CalcShippingFare()
        {
            throw new NotImplementedException();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("{orderID}")]
        public async Task<IActionResult> UpdateOrder(Guid userID, string orderID, [FromBody] OrderUpdateDto order)
        {
            var orderEntity = await orderRepository.GetOrderByID(orderID);
            if (orderEntity == null || !orderEntity.UserID.Equals(userID))
            {
                return NotFound();
            }

            mapper.Map(order, orderEntity);
            orderRepository.UpdateOrder(orderEntity);
            await orderRepository.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPatch("{orderID}")]
        public async Task<IActionResult> PartiallyUpdateOrder(Guid userID, string orderID, [FromBody] OrderPatchDto order)
        {
            throw new NotImplementedException();

        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("{orderID}")]
        public async Task<IActionResult> RemoveOrder(Guid userID, string orderID)
        {
            var order = await orderRepository.GetOrderByID(orderID);
            if (order == null || !order.UserID.Equals(userID))
            {
                return NotFound();
            }

            orderRepository.RemoveOrder(order);
            await orderRepository.SaveAsync();
            return NoContent();
        }
    }
}