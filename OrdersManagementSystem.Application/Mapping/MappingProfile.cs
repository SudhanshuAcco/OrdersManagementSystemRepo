using AutoMapper;
using OrdersManagementSystem.Application.DTOs;
using OrdersManagementSystem.Domain.Model;

namespace OrdersManagementSystem.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        }
    }
}
