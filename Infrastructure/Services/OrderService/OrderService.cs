using Domain.DTO;
using Domain.Models;
using Infrastructure.Repositories;
using Infrastructure.Services.ProductService;
using Infrastructure.Services.UserService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOnlineStoreReadRepository<Order> _orderReadRepository;
        private readonly IOnlineStoreWriteRepository<Order> _orderWriteRepository;

        public OrderService(IOnlineStoreReadRepository<Order> orderReadRepository, 
            IOnlineStoreWriteRepository<Order> orderWriteRepository, IProductService productService, IUserService userService)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _productService = productService;
            _userService = userService;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var orders = await _orderReadRepository.GetAll().ToListAsync();
            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            Guid resultId;
            var isValid = !string.IsNullOrWhiteSpace(orderId) ? Guid.TryParse(orderId, out resultId) : throw new ArgumentNullException("No Order ID was provided.");

            if (!isValid)
            {
                throw new InvalidOperationException($"Invalid format of Order ID: {orderId} provided.");
            }

            var order = await _orderReadRepository.GetAll().FirstOrDefaultAsync(o => o.OrderId == resultId) ?? 
                throw new InvalidOperationException($"Order does not exist. Incorrect Order ID: {orderId} provided.");

            return order;
        }

        public async Task<Order> AddOrder(OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                throw new ArgumentNullException("Order must not be null. Missing request body.");
            }

            await _userService.GetUserByIdAsync(orderDTO.UserId); // check if user exists
            await _productService.GetProductByIdAsync(orderDTO.ProductId); // check if product exists

            Order order = new Order() 
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.Parse(orderDTO.UserId),
                ProductId = Guid.Parse(orderDTO.ProductId),
                PurchaseAmount = orderDTO.PurchaseAmount > 0 ? orderDTO.PurchaseAmount : throw new ArgumentException($"Invalid {nameof(orderDTO.PurchaseAmount)} provided."),
                OrderDate = DateTime.Now,
                ShippingDate = orderDTO.ShippingDate != default(DateTime) ? orderDTO.ShippingDate : throw new InvalidOperationException($"Invalid {nameof(orderDTO.ShippingDate)} provided."),
                Shipped = false,
                PartitionKey = orderDTO.ProductId.ToString()
            };

            return await _orderWriteRepository.AddAsync(order);
        }

        public async Task<Order> ShipOrder(string orderId)
        {
            Order order = await GetOrderByIdAsync(orderId);
            order.Shipped = true;
            order.ShippingDate = DateTime.Now;

            return await _orderWriteRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(string orderId)
        {
            Order order = await GetOrderByIdAsync(orderId);
            await _orderWriteRepository.DeleteAsync(order);
        }
    }
}
