using OrdersManagementSystem.Domain.Model;
namespace OrdersManagementSystem.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetById(Guid id);
        Task Update(Order order);
        Task Create(Order order);
        Task Delete(Guid id);
        Task<IEnumerable<Order>> GetAll();
    }
}
