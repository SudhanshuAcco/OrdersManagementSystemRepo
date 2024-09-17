using AutoMapper;
using OrdersManagementSystem.Domain.Interfaces;
using OrdersManagementSystem.Application.DTOs;
using OrdersManagementSystem.Domain.Model;
using OrdersManagementSystem.Domain.Exceptions;


namespace OrdersManagementSystem.Application.Services
{
    public class OrderService : IGenericService<OrderDTO, Order>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository,  IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderDTO> GetByIdAsync(Guid id)
        {
            id.ValidateNotEmpty(nameof(id));
            var order = await _orderRepository.GetById(id);
            if (order == null) {
                throw new OrderNotFoundException("Order not found.");
            }
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> CreateAsync(OrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO); 
            await _orderRepository.Create(order);
            var createdOrderDTO = _mapper.Map<OrderDTO>(order);
            return createdOrderDTO;
        }

        public async Task DeleteAsync(Guid id)
        {
            id.ValidateNotEmpty(nameof(id));
            var existingOrder = await _orderRepository.GetById(id);
            if (existingOrder == null) {
                throw new OrderNotFoundException("Order not found.");
            }
            await _orderRepository.Delete(id);
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAll();
            return orders.Select(order => _mapper.Map<OrderDTO>(order));
        }

        public async Task UpdateAsync(Guid id, OrderDTO updatedOrder)
        {
            id.ValidateNotEmpty(nameof(id));
            var existingOrder = await _orderRepository.GetById(id);
            if (existingOrder == null) {
                throw new OrderNotFoundException("Order not found.");
            }

            _mapper.Map(updatedOrder, existingOrder);
            await _orderRepository.Update(existingOrder);
        }
    }
}

