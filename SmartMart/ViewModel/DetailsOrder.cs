using SmartMart.Models;

namespace SmartMart.ViewModel
{
    public class DetailsOrder
    {
        public Order order { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
