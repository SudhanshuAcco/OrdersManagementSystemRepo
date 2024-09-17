namespace OrdersManagementSystem.Domain.Model
{
    public class Order
    {
        public Guid OrderID { get;  set; }
        public Guid CustomerID { get; set; }
        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public Order()
        {
            OrderID = Guid.NewGuid();
        }
    }

    public class OrderItem
    {
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
