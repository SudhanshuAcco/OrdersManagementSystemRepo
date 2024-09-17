using Microsoft.AspNetCore.Mvc;
using Moq;
using OrdersManagementSystem.Application.DTOs;
using Xunit;

namespace OrdersManagementSystem.Tests
{
    public class OrdersControllerTests : OrdersControllerTestBase
    {
        [Theory]
        [InlineData("d81b3d65-6ec2-4d7a-9a1d-4a54e4855d38", typeof(OkObjectResult))]
        public async Task GetOrder_ShouldReturnExpectedResult_ID(Guid id, Type expectedResultType)
        {
            // Arrange
            if (id == NonExistingOrderId) {
                // Act
                var result = await Controller.GetOrder(id);

                // Assert
                Assert.IsType(expectedResultType, result);
            } else {
                // Mock service behavior
                var orderDTO = new OrderDTO { OrderID = id };
                OrderServiceMock.Setup(service => service.GetByIdAsync(id))
                                .ReturnsAsync(orderDTO);

                // Act
                var result = await Controller.GetOrder(id);

                // Assert
                var actionResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(orderDTO, actionResult.Value);
            }
        }

        [Theory]
        [InlineData(true)]
        public async Task GetOrder_ShouldReturnExpectedResult(bool orderExists)
        {
            // Arrange
            OrderDTO orderDTO = orderExists ? new OrderDTO { OrderID = ExistingOrderId } : null;

            OrderServiceMock.Setup(service => service.GetByIdAsync(ExistingOrderId))
                            .ReturnsAsync(orderDTO);

            // Act
            var result = await Controller.GetOrder(ExistingOrderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnOrderDTO = Assert.IsType<OrderDTO>(okResult.Value);
            Assert.Equal(ExistingOrderId, returnOrderDTO.OrderID);
            
        }

        [Fact]
        public async Task GetOrder_ShouldReturnOkResult_WithOrderDTO()
        {
            // Arrange
            var orderDTO = new OrderDTO { OrderID = ExistingOrderId };
            OrderServiceMock.Setup(service => service.GetByIdAsync(ExistingOrderId))
                            .ReturnsAsync(orderDTO);

            // Act
            var result = await Controller.GetOrder(ExistingOrderId);

            // Assert
            var returnedOrderDTO = AssertOkResult<OrderDTO>(result);
            Assert.Equal(ExistingOrderId, returnedOrderDTO.OrderID);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnCreatedResult_WithOrderDTO()
        {
            // Arrange
            var orderDTO = new OrderDTO { OrderID = ExistingOrderId };
            OrderServiceMock.Setup(service => service.CreateAsync(orderDTO))
                            .ReturnsAsync(orderDTO);

            // Act
            var result = await Controller.CreateOrder(orderDTO);

            // Assert
            var returnedOrderDTO = AssertCreatedAtActionResult<OrderDTO>(result);
            Assert.Equal(orderDTO.OrderID, returnedOrderDTO.OrderID);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnBadRequest_WhenOrderDTOIsNull()
        {
            // Arrange
            OrderDTO nullOrderDTO = null;

            // Act
            var result = await Controller.CreateOrder(nullOrderDTO);

            // Assert
            AssertBadRequestResult(result, "OrderDTO cannot be null.");
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updatedOrderDTO = new OrderDTO { OrderID = ExistingOrderId };
            OrderServiceMock.Setup(service => service.UpdateAsync(ExistingOrderId, updatedOrderDTO))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await Controller.UpdateOrder(ExistingOrderId, updatedOrderDTO);

            // Assert
            var returnedOrderDTO = AssertOkResult<OrderDTO>(result);
            Assert.Equal(ExistingOrderId, returnedOrderDTO.OrderID);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var mismatchedOrderDTO = new OrderDTO { OrderID = OrderIdMismatch };

            // Act
            var result = await Controller.UpdateOrder(ExistingOrderId, mismatchedOrderDTO);

            // Assert
            AssertBadRequestResult(result, "Order ID mismatch or DTO Null.");
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnBadRequest_WhenOrderDTOIsNull()
        {
            // Arrange
            OrderDTO nullOrderDTO = null;

            // Act
            var result = await Controller.UpdateOrder(ExistingOrderId, nullOrderDTO);

            // Assert
            AssertBadRequestResult(result, "Order ID mismatch or DTO Null.");
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnOk_WithConfirmationMessage()
        {
            // Arrange
            OrderServiceMock.Setup(service => service.DeleteAsync(ExistingOrderId))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await Controller.DeleteOrder(ExistingOrderId);

            // Assert
            OkObjectResult(result);
        }

        [Fact]
        public async Task GetAllOrders_ShouldReturnOkResult_WithListOfOrderDTOs()
        {
            // Arrange
            var ordersDTO = new List<OrderDTO>
        {
            new OrderDTO { OrderID = ExistingOrderId },
            new OrderDTO { OrderID = AnotherFixedOrderId }
        };
            OrderServiceMock.Setup(service => service.GetAllAsync())
                            .ReturnsAsync(ordersDTO);

            // Act
            var result = await Controller.GetAllOrders();

            // Assert
            var returnedOrdersDTO = AssertOkResult<List<OrderDTO>>(result);
            Assert.Equal(ordersDTO.Count, returnedOrdersDTO.Count);
        }
    }
}
