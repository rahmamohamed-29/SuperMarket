using SmartMart.Models;

namespace SmartMart.ViewModel
{
    public class CreateNewCustomers
    {
        public Customer Customer { get; set; } = new Customer();

        public Order Order { get; set; } = new Order();
        public List<OrderItem> OrderItem { get; set; } = new List<OrderItem>();
        
    }
}
