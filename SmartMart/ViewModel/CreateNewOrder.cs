using SmartMart.Models;

namespace SmartMart.ViewModel
{
    public class CreateNewOrder
    {
        public Order Order { get; set; } = new Order();
        public List<OrderItem> OrderItem { get; set; } = new List<OrderItem>();
    }
}
