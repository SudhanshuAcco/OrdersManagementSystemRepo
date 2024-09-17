using OrdersManagementSystem.Domain.Model;

namespace OrdersManagementSystem.Application.DTOs
{
    public class OrderDTO
    {
        public Guid OrderID { get; set; }
        public Guid CustomerID { get; set; }
        public IList<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
    }


    public class OrderItemDTO
    {
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

    
}
