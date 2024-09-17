using Microsoft.AspNetCore.Mvc;
using OrdersManagementSystem.Application.DTOs;
using OrdersManagementSystem.Application.Services;
using OrdersManagementSystem.Domain.Model;

namespace OrdersManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IGenericService<OrderDTO, Order> _orderService;
        public OrdersController(IGenericService<OrderDTO, Order> orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var orderDTO = await _orderService.GetByIdAsync(id);
            if (orderDTO == null) {
                return NotFound();
            }
            return Ok(orderDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null) {
                return BadRequest("OrderDTO cannot be null.");
            }
            var createdOrderDTO = await _orderService.CreateAsync(orderDTO);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrderDTO.OrderID }, createdOrderDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderDTO orderDTO)
        {
            if (id != orderDTO?.OrderID || orderDTO == null) {
                return BadRequest("Order ID mismatch or DTO Null.");
            }
            await _orderService.UpdateAsync(id, orderDTO);
            return Ok(orderDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            await _orderService.DeleteAsync(id);
            return Ok(new { Message = $"Order with ID {id} deleted successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }
    }
}
