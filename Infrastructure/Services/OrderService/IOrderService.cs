using Domain.DTO;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.OrderService
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
       
        Task<Order> AddOrder(OrderDTO order);

        Task<Order> ShipOrder(string orderId);
    }
}