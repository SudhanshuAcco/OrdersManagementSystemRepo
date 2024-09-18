using AutoMapper;
using Moq;
using OrdersManagementSystem.Application.DTOs;
using OrdersManagementSystem.Application.Mapping;
using OrdersManagementSystem.Application.Services;
using OrdersManagementSystem.Domain.Interfaces;
using OrdersManagementSystem.Domain.Model;

namespace OrdersManagementSystem.Tests
{
    public abstract class TestBase: IDisposable
    {
        protected readonly Mock<IOrderRepository> _orderRepositoryMock;
        protected readonly IMapper _mapper;
        protected readonly OrderService _orderService;
        protected readonly Guid ExistingOrderId = Guid.Parse("d81b3d65-6ec2-4d7a-9a1d-4a54e4855d38");
        protected readonly Guid NonExistingOrderId = Guid.Parse("00000000-0000-0000-0000-000000000000");
        protected readonly Guid OrderIdMismatch = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        protected readonly Guid AnotherFixedOrderId = Guid.Parse("aabbccdd-1234-5678-90ab-cdef12345678");

        protected TestBase()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = mapperConfig.CreateMapper();
            _orderService = new OrderService(_orderRepositoryMock.Object, _mapper);
        }

        protected OrderDTO GetTestOrderDTO(Guid id)
        {
            return new OrderDTO
            {
                OrderID = id,
                CustomerID = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItemDTO>
            {
                new OrderItemDTO
                {
                    ProductID = Guid.NewGuid(),
                    Quantity = 1,
                    TotalPrice = 100
                }
            }
            };
        }

        protected Order GetTestOrder(Guid id)
        {
            return new Order
            {
                OrderID = id,
                CustomerID = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductID = Guid.NewGuid(),
                    Quantity = 1,
                    TotalPrice = 100
                }
            }
            };
        }

        public void Dispose()
        {
            _orderRepositoryMock.VerifyAll();
        }
    }
}
