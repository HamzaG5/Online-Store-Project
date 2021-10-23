using Domain;
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

        public async Task<Order> AddOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("Order must not be null.");
            }

            order.OrderId = Guid.NewGuid();
            return await _orderWriteRepository.AddAsync(order);
        }
    }
}
