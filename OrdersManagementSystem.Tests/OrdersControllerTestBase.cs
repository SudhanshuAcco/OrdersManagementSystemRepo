using Microsoft.AspNetCore.Mvc;
using Moq;
using OrdersManagementSystem.API.Controllers;
using OrdersManagementSystem.Application.DTOs;
using OrdersManagementSystem.Application.Services;
using OrdersManagementSystem.Domain.Model;
using Xunit;

namespace OrdersManagementSystem.Tests
{
    public abstract class OrdersControllerTestBase : IDisposable
    {
        protected readonly Mock<IGenericService<OrderDTO, Order>> OrderServiceMock;
        protected readonly OrdersController Controller;       
        protected readonly Guid ExistingOrderId = Guid.Parse("d81b3d65-6ec2-4d7a-9a1d-4a54e4855d38");
        protected readonly Guid NonExistingOrderId = Guid.Parse("00000000-0000-0000-0000-000000000000");
        protected readonly Guid OrderIdMismatch = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        protected readonly Guid AnotherFixedOrderId = Guid.Parse("aabbccdd-1234-5678-90ab-cdef12345678"); 

        protected OrdersControllerTestBase()
        {
            OrderServiceMock = new Mock<IGenericService<OrderDTO, Order>>();
            Controller = new OrdersController(OrderServiceMock.Object);
        }

        protected T AssertOkResult<T>(IActionResult result) where T : class
        {
            var okResult = Assert.IsType<OkObjectResult>(result);
            return Assert.IsType<T>(okResult.Value);
        }

        protected void AssertNotFoundResult(IActionResult result)
        {
            Assert.IsType<NotFoundResult>(result);
        }

        protected T AssertCreatedAtActionResult<T>(IActionResult result) where T : class
        {
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            return Assert.IsType<T>(createdResult.Value);
        }

        protected void AssertBadRequestResult(IActionResult result, string expectedMessage)
        {
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedMessage, badRequestResult.Value);
        }
        protected void OkObjectResult(IActionResult result)
        {
            Assert.IsType<OkObjectResult>(result);
        }
        public void Dispose()
        {
            OrderServiceMock.VerifyAll();
        }
    }
}
