using Domain.DTO;
using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOnlineStoreReadRepository<Order> _orderReadRepository;
        private readonly IOnlineStoreWriteRepository<Order> _orderWriteRepository;

        public OrderService(IOnlineStoreReadRepository<Order> orderReadRepository, 
            IOnlineStoreWriteRepository<Order> orderWriteRepository)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var donations = await _orderReadRepository.GetAll().ToListAsync() ?? throw new Exception("Error retrieving orders.");
            return donations;
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            try
            {
                Guid resultId = !string.IsNullOrWhiteSpace(orderId) ? Guid.Parse(orderId) : throw new ArgumentNullException("No Order ID was provided.");

                var order = await _orderReadRepository.GetAll().FirstOrDefaultAsync(o => o.OrderId == resultId) ?? 
                    throw new InvalidOperationException($"Order does not exist. Incorrect Order ID: {orderId} provided.");

                return order;
            }
            catch
            {
                throw new InvalidOperationException($"Invalid format of Order ID: {orderId} provided.");
            }
        }

        public async Task<Order> AddOrder(OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                throw new ArgumentNullException("Order must not be null.");
            }

            var order = new Order()
            {
                OrderId = Guid.NewGuid(),
                ProductId = orderDTO.ProductId,
                OrderDate = DateTime.Now,
                ShippingDate = orderDTO.ShippingDate,
                Shipped = false,
                PartitionKey = orderDTO.ProductId.ToString()
            };

            return await _orderWriteRepository.AddAsync(order);
        }

        public async Task<Order> ShipOrder(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new NullReferenceException($"{nameof(orderId)} must be provided.");
            }

            Order order = await GetOrderByIdAsync(orderId);
            order.Shipped = true;
            order.ShippingDate = DateTime.Now;

            return await _orderWriteRepository.Update(order);
        }
    }
}
