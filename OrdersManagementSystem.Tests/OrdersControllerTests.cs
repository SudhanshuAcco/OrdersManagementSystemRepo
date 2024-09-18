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
            if (id == NonExistingOrderId) {
                var result = await Controller.GetOrder(id);
                Assert.IsType(expectedResultType, result);
            } else {
                var orderDTO = new OrderDTO { OrderID = id };
                OrderServiceMock.Setup(service => service.GetByIdAsync(id))
                                .ReturnsAsync(orderDTO);
                var result = await Controller.GetOrder(id);
                var actionResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(orderDTO, actionResult.Value);
            }
        }

        [Theory]
        [InlineData(true)]
        public async Task GetOrder_ShouldReturnExpectedResult(bool orderExists)
        {
            OrderDTO orderDTO = orderExists ? new OrderDTO { OrderID = ExistingOrderId } : null;
            OrderServiceMock.Setup(service => service.GetByIdAsync(ExistingOrderId))
                            .ReturnsAsync(orderDTO);
            var result = await Controller.GetOrder(ExistingOrderId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnOrderDTO = Assert.IsType<OrderDTO>(okResult.Value);
            Assert.Equal(ExistingOrderId, returnOrderDTO.OrderID);            
        }

        [Fact]
        public async Task GetOrder_ShouldReturnOkResult_WithOrderDTO()
        {
            var orderDTO = new OrderDTO { OrderID = ExistingOrderId };
            OrderServiceMock.Setup(service => service.GetByIdAsync(ExistingOrderId))
                            .ReturnsAsync(orderDTO);
            var result = await Controller.GetOrder(ExistingOrderId);
            var returnedOrderDTO = AssertOkResult<OrderDTO>(result);
            Assert.Equal(ExistingOrderId, returnedOrderDTO.OrderID);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnCreatedResult_WithOrderDTO()
        {
            var orderDTO = new OrderDTO { OrderID = ExistingOrderId };
            OrderServiceMock.Setup(service => service.CreateAsync(orderDTO))
                            .ReturnsAsync(orderDTO);
            var result = await Controller.CreateOrder(orderDTO);
            var returnedOrderDTO = AssertCreatedAtActionResult<OrderDTO>(result);
            Assert.Equal(orderDTO.OrderID, returnedOrderDTO.OrderID);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnBadRequest_WhenOrderDTOIsNull()
        {
            OrderDTO nullOrderDTO = null;
            var result = await Controller.CreateOrder(nullOrderDTO);
            AssertBadRequestResult(result, "OrderDTO cannot be null.");
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnOkResult_WhenUpdateIsSuccessful()
        {
            var updatedOrderDTO = new OrderDTO { OrderID = ExistingOrderId };
            OrderServiceMock.Setup(service => service.UpdateAsync(ExistingOrderId, updatedOrderDTO))
                            .Returns(Task.CompletedTask);
            var result = await Controller.UpdateOrder(ExistingOrderId, updatedOrderDTO);
            var returnedOrderDTO = AssertOkResult<OrderDTO>(result);
            Assert.Equal(ExistingOrderId, returnedOrderDTO.OrderID);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnBadRequest_WhenIdDoesNotMatch()
        {
            var mismatchedOrderDTO = new OrderDTO { OrderID = OrderIdMismatch };
            var result = await Controller.UpdateOrder(ExistingOrderId, mismatchedOrderDTO);
            AssertBadRequestResult(result, "Order ID mismatch or DTO Null.");
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnBadRequest_WhenOrderDTOIsNull()
        {
            OrderDTO nullOrderDTO = null;
            var result = await Controller.UpdateOrder(ExistingOrderId, nullOrderDTO);
            AssertBadRequestResult(result, "Order ID mismatch or DTO Null.");
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnOk_WithConfirmationMessage()
        {
            OrderServiceMock.Setup(service => service.DeleteAsync(ExistingOrderId))
                            .Returns(Task.CompletedTask);
            var result = await Controller.DeleteOrder(ExistingOrderId);
            OkObjectResult(result);
        }

        [Fact]
        public async Task GetAllOrders_ShouldReturnOkResult_WithListOfOrderDTOs()
        {
            var ordersDTO = new List<OrderDTO>
        {
            new OrderDTO { OrderID = ExistingOrderId },
            new OrderDTO { OrderID = AnotherFixedOrderId }
        };
            OrderServiceMock.Setup(service => service.GetAllAsync())
                            .ReturnsAsync(ordersDTO);
            var result = await Controller.GetAllOrders();
            var returnedOrdersDTO = AssertOkResult<List<OrderDTO>>(result);
            Assert.Equal(ordersDTO.Count, returnedOrdersDTO.Count);
        }
    }
}
