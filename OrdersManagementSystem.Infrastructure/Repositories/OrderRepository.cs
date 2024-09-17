using OrdersManagementSystem.Domain.Interfaces;
using OrdersManagementSystem.Domain.Model;
using System.Collections.Concurrent;

namespace OrdersManagementSystem.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static readonly ConcurrentDictionary<Guid, Order> _orders = new();

        public Task<IEnumerable<Order>> GetAll()
        {
            return Task.FromResult(_orders.Values.AsEnumerable());
        }

        public Task<Order> GetById(Guid id)
        {
            _orders.TryGetValue(id, out var order);
            return Task.FromResult(order);
        }

        public Task Update(Order order)
        {
            _orders[order.OrderID] = order;
            return Task.CompletedTask;
        }

        public Task Delete(Guid id)
        {
            _orders.TryRemove(id, out _);
            return Task.CompletedTask;
        }

        public Task Create(Order order)
        {
            _orders.TryAdd(order.OrderID, order);
            return Task.CompletedTask;
        }
    }

}
