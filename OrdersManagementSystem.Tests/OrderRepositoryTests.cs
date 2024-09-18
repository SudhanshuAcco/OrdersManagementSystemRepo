using OrdersManagementSystem.Domain.Model;
using OrdersManagementSystem.Infrastructure.Repositories;
using Xunit;

namespace OrdersManagementSystem.Tests
{
    public class OrderRepositoryTests : TestBase
    {
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            _repository = new OrderRepository();
        }

        [Fact]
        public async Task Create_ShouldAddOrder()
        {
            var order = GetTestOrder(ExistingOrderId); 
            await _repository.Create(order);
            var result = await _repository.GetById(order.OrderID);
            Assert.NotNull(result);
            Assert.Equal(order.OrderID, result.OrderID);
        }

        [Fact]
        public async Task GetById_ShouldReturnOrder_WhenOrderExists()
        {
            var order = GetTestOrder(ExistingOrderId);
            await _repository.Create(order);
            var result = await _repository.GetById(order.OrderID);
            Assert.NotNull(result);
            Assert.Equal(order.OrderID, result.OrderID);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            var id = NonExistingOrderId;
            var result = await _repository.GetById(id);
            Assert.Null(result);
        }

        [Fact]
        public async Task Update_ShouldUpdateExistingOrder()
        {
            var order = GetTestOrder(ExistingOrderId);
            await _repository.Create(order);

            var updatedOrder = new Order
            {
                OrderID = order.OrderID,
                CustomerID = Guid.NewGuid(), 
                OrderDate = DateTime.UtcNow.AddDays(1),
                Status = OrderStatus.Pending,
                OrderItems = order.OrderItems
            };

            await _repository.Update(updatedOrder);
            var result = await _repository.GetById(order.OrderID);
            Assert.NotNull(result);
            Assert.Equal(updatedOrder.CustomerID, result.CustomerID);
            Assert.Equal(updatedOrder.OrderDate, result.OrderDate);
            Assert.Equal(updatedOrder.Status, result.Status);
        }

        [Fact]
        public async Task Delete_ShouldRemoveOrder()
        {
            var order = GetTestOrder(ExistingOrderId);
            await _repository.Create(order);
            await _repository.Delete(order.OrderID);
            var result = await _repository.GetById(order.OrderID);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllOrders()
        {
            var order1 = GetTestOrder(ExistingOrderId); 
            var order2 = GetTestOrder(AnotherFixedOrderId);
            await _repository.Create(order1);
            await _repository.Create(order2);
            var orders = await _repository.GetAll();
            Assert.NotEmpty(orders);
            Assert.Contains(orders, o => o.OrderID == order1.OrderID);
            Assert.Contains(orders, o => o.OrderID == order2.OrderID);
        }
    }
}
