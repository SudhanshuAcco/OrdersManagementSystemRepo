using AutoMapper;
using OrdersManagementSystem.Application.DTOs;
using OrdersManagementSystem.Domain.Model;

namespace OrdersManagementSystem.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Explicitly map Order and OrderDTO, including the enum conversion
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap(); // Maps OrderDTO to Order, including enum conversion

            CreateMap<OrderItem, OrderItemDTO>()
                .ReverseMap(); // Maps OrderItemDTO to OrderItem
        }
    }
}
