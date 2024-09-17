using Moq;
using OrdersManagementSystem.Application.DTOs;
using OrdersManagementSystem.Domain.Exceptions;
using OrdersManagementSystem.Domain.Model;
using Xunit;

namespace OrdersManagementSystem.Tests
{
    public class OrderServiceTests: TestBase
    {
        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectOrderDTO_WhenOrderExists()
        {
            // Arrange
            var orderId = ExistingOrderId;
            var order = GetTestOrder(orderId);
            var orderDTO = _mapper.Map<OrderDTO>(order);

            _orderRepositoryMock.Setup(repo => repo.GetById(orderId))
                                .ReturnsAsync(order);

            // Act
            var result = await _orderService.GetByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDTO.OrderID, result.OrderID);
        }

        [Theory]
        [InlineData("d81b3d65-6ec2-4d7a-9a1d-4a54e4855d38")]
        [InlineData("aabbccdd-1234-5678-90ab-cdef12345678")]
        public async Task GetByIdAsync_ShouldReturnOrderDTO_WhenDifferentValidOrderIDs(Guid orderId)
        {
            // Arrange
            var order = GetTestOrder(orderId);
            var orderDTO = _mapper.Map<OrderDTO>(order);

            _orderRepositoryMock.Setup(repo => repo.GetById(orderId))
                                .ReturnsAsync(order);

            // Act
            var result = await _orderService.GetByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDTO.OrderID, result.OrderID);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowArgumentException_WhenOrderIdIsEmpty()
        {
            // Arrange
            var orderId = NonExistingOrderId;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _orderService.GetByIdAsync(orderId));
        }        

        [Fact]
        public async Task GetByIdAsync_ShouldThrowOrderNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = AnotherFixedOrderId;
            _orderRepositoryMock.Setup(repo => repo.GetById(orderId))
                                .ReturnsAsync((Order) null);

            // Act & Assert
            await Assert.ThrowsAsync<OrderNotFoundException>(() => _orderService.GetByIdAsync(orderId));
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedOrderDTO_WhenOrderDTOIsValid()
        {
            // Arrange
            var orderDTO = GetTestOrderDTO(ExistingOrderId); // Use a fixed GUID for consistency
            var order = _mapper.Map<Order>(orderDTO);

            // Mock repository to return no errors
            _orderRepositoryMock.Setup(repo => repo.Create(It.Is<Order>(o => o.OrderID == orderDTO.OrderID)))
                                .Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.CreateAsync(orderDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDTO.OrderID, result.OrderID);
            Assert.Equal(orderDTO.CustomerID, result.CustomerID);
            Assert.Equal(orderDTO.OrderDate, result.OrderDate);
            Assert.Equal(orderDTO.Status, result.Status);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var orderDTO = GetTestOrderDTO(ExistingOrderId); // Use a fixed GUID for consistency
            var order = _mapper.Map<Order>(orderDTO);

            // Mock repository to throw an exception
            _orderRepositoryMock.Setup(repo => repo.Create(It.Is<Order>(o => o.OrderID == orderDTO.OrderID)))
                                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _orderService.CreateAsync(orderDTO));

            // Additional assertion to check the exception message
            Assert.Equal("Database error", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedOrderDTO_WhenValidOrderDTOIsProvided()
        {
            // Arrange
            var orderDTO = GetTestOrderDTO(ExistingOrderId); // Use a fixed GUID for consistency
            var order = _mapper.Map<Order>(orderDTO);

            // Mock repository to return no errors
            _orderRepositoryMock.Setup(repo => repo.Create(It.Is<Order>(o => o.OrderID == orderDTO.OrderID)))
                                .Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.CreateAsync(orderDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDTO.OrderID, result.OrderID);
            Assert.Equal(orderDTO.CustomerID, result.CustomerID);
            Assert.Equal(orderDTO.OrderDate, result.OrderDate);
            Assert.Equal(orderDTO.Status, result.Status);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedOrderDTO()
        {
            // Arrange
            var orderDTO = GetTestOrderDTO(ExistingOrderId); 
            var order = _mapper.Map<Order>(orderDTO); 

            // Set up the repository mock
            _orderRepositoryMock.Setup(repo => repo.Create(It.Is<Order>(o => o.OrderID == orderDTO.OrderID)))
                                .Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.CreateAsync(orderDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDTO.OrderID, result.OrderID);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = ExistingOrderId;
            var existingOrder = GetTestOrder(orderId);
            var updatedOrderDTO = GetTestOrderDTO(orderId);

            _orderRepositoryMock.Setup(repo => repo.GetById(orderId))
                                .ReturnsAsync(existingOrder);
            _orderRepositoryMock.Setup(repo => repo.Update(It.IsAny<Order>()))
                                .Returns(Task.CompletedTask);

            // Act
            await _orderService.UpdateAsync(orderId, updatedOrderDTO);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.Update(It.Is<Order>(o => o.OrderID == orderId)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = ExistingOrderId;
            var existingOrder = GetTestOrder(orderId);

            _orderRepositoryMock.Setup(repo => repo.GetById(orderId))
                                .ReturnsAsync(existingOrder);
            _orderRepositoryMock.Setup(repo => repo.Delete(orderId))
                                .Returns(Task.CompletedTask);

            // Act
            await _orderService.DeleteAsync(orderId);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.Delete(orderId), Times.Once);
        }

       
    }
}
