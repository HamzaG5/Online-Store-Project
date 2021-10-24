using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
       
        Task<Order> AddOrder(Order order);

        Task<Order> ShipOrder(string orderId);
    }
}