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
            _repository = new OrderRepository(); // Initialize the repository with real implementation
        }

        [Fact]
        public async Task Create_ShouldAddOrder()
        {
            // Arrange
            var order = GetTestOrder(ExistingOrderId); // Create a new Order with a unique GUID

            // Act
            await _repository.Create(order);

            // Assert
            var result = await _repository.GetById(order.OrderID);
            Assert.NotNull(result);
            Assert.Equal(order.OrderID, result.OrderID);
        }

        [Fact]
        public async Task GetById_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var order = GetTestOrder(ExistingOrderId);
            await _repository.Create(order);

            // Act
            var result = await _repository.GetById(order.OrderID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.OrderID, result.OrderID);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var id = NonExistingOrderId;

            // Act
            var result = await _repository.GetById(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update_ShouldUpdateExistingOrder()
        {
            // Arrange
            var order = GetTestOrder(ExistingOrderId);
            await _repository.Create(order);

            var updatedOrder = new Order
            {
                OrderID = order.OrderID,
                CustomerID = Guid.NewGuid(), 
                OrderDate = DateTime.UtcNow.AddDays(1),
                Status = "Updated",
                OrderItems = order.OrderItems
            };

            // Act
            await _repository.Update(updatedOrder);
            var result = await _repository.GetById(order.OrderID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedOrder.CustomerID, result.CustomerID);
            Assert.Equal(updatedOrder.OrderDate, result.OrderDate);
            Assert.Equal(updatedOrder.Status, result.Status);
        }

        [Fact]
        public async Task Delete_ShouldRemoveOrder()
        {
            // Arrange
            var order = GetTestOrder(ExistingOrderId);
            await _repository.Create(order);

            // Act
            await _repository.Delete(order.OrderID);
            var result = await _repository.GetById(order.OrderID);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllOrders()
        {
            // Arrange
            var order1 = GetTestOrder(ExistingOrderId); // Use fixed GUID here
            var order2 = GetTestOrder(AnotherFixedOrderId);
            await _repository.Create(order1);
            await _repository.Create(order2);

            // Act
            var orders = await _repository.GetAll();

            // Assert
            Assert.NotEmpty(orders);
            Assert.Contains(orders, o => o.OrderID == order1.OrderID);
            Assert.Contains(orders, o => o.OrderID == order2.OrderID);
        }
    }
}
